using System.Collections.Generic;
using System.Linq;
using Lykke.Service.CryptoIndex.Client;

namespace Lykke.Service.HeatmapDataWriter.DomainServices
{
    public class CryptoIndexClientManager
    {
        private Dictionary<string, ICryptoIndexClient> _clients = new Dictionary<string, ICryptoIndexClient>();

        public const string LCI = "LCI";
        public const string PLCI = "PLCI";
        public const string SCLCI = "SCLCI";

        public static CryptoIndexClientManager Create() => new CryptoIndexClientManager();

        public CryptoIndexClientManager AddClient(string name, ICryptoIndexClient client)
        {
            _clients.Add(name, client);
            return this;
        }

        public ICryptoIndexClient Get(string name)
        {
            if (_clients.TryGetValue(name, out var client))
            {
                return client;
            }

            return null;
        }

        public IReadOnlyDictionary<string, ICryptoIndexClient> GetAll() => _clients;
    }
}
