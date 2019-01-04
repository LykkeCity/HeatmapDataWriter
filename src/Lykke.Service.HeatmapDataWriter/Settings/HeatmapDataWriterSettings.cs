using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HeatmapDataWriter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class HeatmapDataWriterSettings
    {
        public DbSettings Db { get; set; }
    }
}
