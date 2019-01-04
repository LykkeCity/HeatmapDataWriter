using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.CryptoIndex.Client;
using Lykke.Service.Dwh.Client;

namespace Lykke.Service.HeatmapDataWriter.DomainServices
{
    public class LoadDataJob : IStartable, IStopable
    {
        private readonly CryptoIndexClientManager _cryptoIndexClientManager;
        private readonly IDwhClient _dwhClient;
        private ILog _log;
        private TimerTrigger _timer;

        public LoadDataJob(CryptoIndexClientManager cryptoIndexClientManager, IDwhClient dwhClient, ILogFactory logFactory)
        {
            _cryptoIndexClientManager = cryptoIndexClientManager;
            _dwhClient = dwhClient;
            _log = logFactory.CreateLog(this);
            _timer = new TimerTrigger(nameof(LoadDataJob), TimeSpan.FromMinutes(1), logFactory, DoTimer);
        }

        public void Start()
        {
            Console.WriteLine("Start!!!");
            _timer.Start();
        }

        private async Task DoTimer(ITimerTrigger timer, TimerTriggeredHandlerArgs args, CancellationToken cancellationtoken)
        {
            foreach (var client in _cryptoIndexClientManager.GetAll())
            {
                await LoadCryptoIndex(client.Key, client.Value);
            }

            await LoadMarketCupData();
            await LoadPriceHistory();
        }

        private async Task LoadPriceHistory()
        {
            //todo: сделаю позже выгрузку заданного количества точек по графику цены топ 40 ассетов
        }

        private async Task LoadMarketCupData()
        {
            // get data
            var dataset = await _dwhClient.Call(new Dictionary<string, string>(), "dbo.HM_CoinmarketcapData", "report");
            var reader = dataset.Data.Tables[0].CreateDataReader();

            var data = new List<CoinmarketcupData>();
            while (reader.Read())
            {
                var item = new CoinmarketcupData();
                item.Asset = reader.GetString(0);
                item.Price = (decimal)reader.GetDouble(1);
                item.Volume24h = (decimal)reader.GetDouble(2);
                item.MarketCapUsd = (decimal)reader.GetDouble(3);
                item.PercentChange1h = (decimal)reader.GetDouble(4);
                item.PercentChange24h = (decimal)reader.GetDouble(5);
                item.PercentChange7d = (decimal)reader.GetDouble(6);
                data.Add(item);
            }

            // report
            Console.WriteLine("===== coinmarketcup top 10 =====");
            foreach (var item in data.OrderByDescending(e => e.MarketCapUsd).Take(10))
            {
                Console.WriteLine($"  {item.Asset}, Volume24h: {item.Volume24h:N0}$, change 1h: {item.PercentChange1h:N2}%, change1d: {item.PercentChange24h:N2}%");
            }
        }

        private async Task LoadCryptoIndex(string name, ICryptoIndexClient client)
        {
            var dict = new Dictionary<string, AssetInfo>();
            var indexValue = 0m;
            var indexChangeToday = 0m;

            #region load data

            var data = await client.Public.GetLastAsync();
            var change = await client.Public.GetChangeAsync();

            indexValue = data.Value;
            indexChangeToday = (change[1].Item2 - change[0].Item2) / change[0].Item2;

            foreach (var price in data.MiddlePrices)
            {
                if (!dict.TryGetValue(price.Key, out var item))
                    item = new AssetInfo(price.Key);

                item.Price = price.Value;
                dict[item.Asset] = item;
            }

            foreach (var cup in data.MarketCaps)
            {
                if (!dict.TryGetValue(cup.Asset, out var item))
                    item = new AssetInfo(cup.Asset);

                item.MarketCap = cup.MarketCap.Value;
                dict[item.Asset] = item;
            }

            foreach (var weight in data.Weights)
            {
                if (!dict.TryGetValue(weight.Key, out var item))
                    item = new AssetInfo(weight.Key);

                item.Weight = weight.Value;
                dict[item.Asset] = item;
            }
            #endregion

            #region report data

            Console.WriteLine($"===== {name} =====");
            Console.WriteLine($"Index value: {indexValue}, change today: {indexChangeToday:P2}");
            foreach (var item in dict.Values.OrderByDescending(e => e.Weight))
            {
                Console.WriteLine($"  {item.Asset}: Price={item.Price}; \tWeight={item.Weight:P2}; \tMarketCup={item.MarketCap:N0}$");
            }
            Console.WriteLine();

            #endregion
        }

        public void Dispose()
        {
            _timer?.Stop();
        }

        public void Stop()
        {
            _timer?.Stop();
        }

        public class AssetInfo
        {
            public AssetInfo(string asset)
            {
                Asset = asset;
            }

            public string Asset { get; set; }
            public decimal Price { get; set; }
            public decimal Weight { get; set; }
            public decimal MarketCap { get; set; }
        }

        public class CoinmarketcupData
        {
            public string Asset { get; set; }
            public decimal Price { get; set; }
            public decimal Volume24h { get; set; }
            public decimal MarketCapUsd { get; set; }
            public decimal PercentChange1h { get; set; }
            public decimal PercentChange24h { get; set; }
            public decimal PercentChange7d { get; set; }
        }
    }
}
