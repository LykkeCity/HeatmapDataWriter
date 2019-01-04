using JetBrains.Annotations;

namespace Lykke.Service.HeatmapDataWriter.Client
{
    /// <summary>
    /// HeatmapDataWriter client interface.
    /// </summary>
    [PublicAPI]
    public interface IHeatmapDataWriterClient
    {
        // Make your app's controller interfaces visible by adding corresponding properties here.
        // NO actual methods should be placed here (these go to controller interfaces, for example - IHeatmapDataWriterApi).
        // ONLY properties for accessing controller interfaces are allowed.

        /// <summary>Application Api interface</summary>
        IHeatmapDataWriterApi Api { get; }
    }
}
