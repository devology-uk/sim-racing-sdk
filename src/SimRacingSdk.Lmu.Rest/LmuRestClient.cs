using System.Diagnostics;
using System.Text;
using SimRacingSdk.Lmu.Rest.Abstractions;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Rest;

// LMU only listens on loopback and stops answering while the game is inactive, so a short timeout
// keeps a non-responding API from stalling callers.
public class LmuRestClient : ILmuRestClient
{
    public const string DefaultBaseAddress = "http://localhost:6397";

    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

    private readonly HttpClient httpClient;

    public LmuRestClient(string baseAddress = DefaultBaseAddress)
    {
        this.httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress),
            Timeout = RequestTimeout
        };
    }

    public Uri BaseAddress => this.httpClient.BaseAddress!;

    public async Task<LmuRestResponse> SendAsync(HttpMethod method, string path, string? body = null,
        CancellationToken cancellationToken = default)
    {
        var timeStamp = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew();
        try
        {
            using var request = CreateRequest(method, path, body);
            using var response = await this.httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            return new LmuRestResponse
            {
                Method = method,
                Path = path,
                TimeStamp = timeStamp,
                Elapsed = stopwatch.Elapsed,
                StatusCode = response.StatusCode,
                ContentType = response.Content.Headers.ContentType?.MediaType,
                Body = responseBody,
                ContentLength = Encoding.UTF8.GetByteCount(responseBody)
            };
        }
        catch(Exception exception) when(exception is HttpRequestException or TaskCanceledException)
        {
            return new LmuRestResponse
            {
                Method = method,
                Path = path,
                TimeStamp = timeStamp,
                Elapsed = stopwatch.Elapsed,
                Error = DescribeFailure(exception)
            };
        }
    }

    public void Dispose()
    {
        this.httpClient.Dispose();
        GC.SuppressFinalize(this);
    }

    private static HttpRequestMessage CreateRequest(HttpMethod method, string path, string? body)
    {
        var request = new HttpRequestMessage(method, path.TrimStart('/'));
        if(!string.IsNullOrWhiteSpace(body))
        {
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        }

        return request;
    }

    private static string DescribeFailure(Exception exception)
    {
        return exception is TaskCanceledException
            ? $"No response within {RequestTimeout.TotalSeconds:0} s"
            : exception.Message;
    }
}
