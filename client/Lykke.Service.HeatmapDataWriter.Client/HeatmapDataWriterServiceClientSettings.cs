using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.HeatmapDataWriter.Client 
{
    /// <summary>
    /// HeatmapDataWriter client settings.
    /// </summary>
    public class HeatmapDataWriterServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
