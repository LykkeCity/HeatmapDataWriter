using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.Dwh.Client;

namespace Lykke.Service.HeatmapDataWriter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public HeatmapDataWriterSettings HeatmapDataWriterService { get; set; }
        public DwhServiceClientSettings DwhServiceClient { get; set; }
        public CryptoIndexServiceClientInstancesSettings CryptoIndexServiceClient { get; set; }
    }



    public class CryptoIndexServiceClientInstancesSettings
    {
        public CryptoIndexClientSettings[] Instances { get; set; }
    }

    public class CryptoIndexClientSettings
    {
        public string DisplayName { get; set; }

        public string ServiceUrl { get; set; }
    }
}
