using Lykke.HttpClientGenerator;

namespace Lykke.Service.HeatmapDataWriter.Client
{
    /// <summary>
    /// HeatmapDataWriter API aggregating interface.
    /// </summary>
    public class HeatmapDataWriterClient : IHeatmapDataWriterClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to HeatmapDataWriter Api.</summary>
        public IHeatmapDataWriterApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public HeatmapDataWriterClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IHeatmapDataWriterApi>();
        }
    }
}
