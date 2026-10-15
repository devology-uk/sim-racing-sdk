using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

public static class ResponseSummary
{
    public static string Describe(LmuRestResponse response)
    {
        var outcome = response.IsResponding
            ? $"{(int)response.StatusCode!} {response.StatusCode}"
            : $"FAILED ({response.Error})";
        return $"{outcome}  {response.Elapsed.TotalMilliseconds:0} ms  {response.ContentLength} bytes  "
            + $"{response.Method.Method} {response.Path}";
    }
}
