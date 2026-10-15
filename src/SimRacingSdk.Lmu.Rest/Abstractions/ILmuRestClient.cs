using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Rest.Abstractions;

public interface ILmuRestClient : IDisposable
{
    Uri BaseAddress { get; }
    Task<LmuRestResponse> SendAsync(HttpMethod method, string path, string? body = null,
        CancellationToken cancellationToken = default);
}
