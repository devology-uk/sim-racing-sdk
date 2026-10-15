using System.Net;

namespace SimRacingSdk.Lmu.Rest.Models;

public record LmuRestResponse
{
    public required HttpMethod Method { get; init; }
    public required string Path { get; init; }
    public required DateTime TimeStamp { get; init; }
    public required TimeSpan Elapsed { get; init; }
    public HttpStatusCode? StatusCode { get; init; }
    public string? ContentType { get; init; }
    public string Body { get; init; } = string.Empty;
    public long ContentLength { get; init; }
    public string? Error { get; init; }

    public bool IsResponding => this.StatusCode != null;
    public bool IsSuccess => this.StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices;
}
