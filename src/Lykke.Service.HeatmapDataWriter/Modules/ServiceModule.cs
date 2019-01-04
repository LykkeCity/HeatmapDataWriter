using Autofac;
using Lykke.Service.CryptoIndex.Client;
using Lykke.Service.Dwh.Client;
using Lykke.Service.HeatmapDataWriter.DomainServices;
using Lykke.Service.HeatmapDataWriter.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.HeatmapDataWriter.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Do not register entire settings in container, pass necessary settings to services which requires them

            // Lykke.Service.Dwh.Client.IDwhClient
            builder.RegisterLykkeServiceClient(_appSettings.CurrentValue.DwhServiceClient, null);

            builder.RegisterType<LoadDataJob>()
                .As<IStartable>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();

            var clientManager = CryptoIndexClientManager.Create();
            foreach (var clientSetting in _appSettings.CurrentValue.CryptoIndexServiceClient.Instances)
            {
                clientManager.AddClient(clientSetting.DisplayName, CreateCryptoIndexClient(clientSetting.ServiceUrl));
            }

            builder.RegisterInstance(clientManager).As<CryptoIndexClientManager>().SingleInstance();
        }

        private ICryptoIndexClient CreateCryptoIndexClient(string url)
        {
            var generator = Lykke.HttpClientGenerator.HttpClientGenerator.BuildForUrl(url)
                .WithAdditionalCallsWrapper(new Lykke.HttpClientGenerator.Infrastructure.ExceptionHandlerCallsWrapper())
                .WithoutRetries()
                .WithoutCaching()
                .Create();

            var client = new CryptoIndexClient(generator);

            return client;
        }
    }
}
