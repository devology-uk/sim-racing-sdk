using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Rest.Abstractions;

public interface ILmuRestEndpointCatalog
{
    IReadOnlyList<LmuRestEndpoint> All { get; }
    IEnumerable<LmuRestEndpoint> RealtimeReads { get; }
    IEnumerable<LmuRestEndpoint> SafeReads { get; }
}
