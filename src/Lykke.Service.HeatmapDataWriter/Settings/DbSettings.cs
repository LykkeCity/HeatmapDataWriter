using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HeatmapDataWriter.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
