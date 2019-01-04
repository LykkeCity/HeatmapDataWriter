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
    }
}
