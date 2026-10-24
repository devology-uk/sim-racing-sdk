using System.Text.Json;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Rest;

public static class LmuRestJson
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public static T? TryRead<T>(LmuRestResponse response) where T : class
    {
        if(!response.IsSuccess || string.IsNullOrWhiteSpace(response.Body))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(response.Body, Options);
        }
        catch(JsonException)
        {
            return null;
        }
    }
}
