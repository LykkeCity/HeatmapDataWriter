using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.HeatmapDataWriter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public HeatmapDataWriterSettings HeatmapDataWriterService { get; set; }
    }
}
