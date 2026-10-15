using System.Text.RegularExpressions;

namespace SimRacingSdk.Lmu.Rest.Models;

public partial record LmuRestEndpoint(string Group, HttpMethod Method, string PathTemplate, LmuRestEndpointKind Kind)
{
    public string DisplayName => $"{this.Method.Method} {this.PathTemplate}";

    public bool AcceptsBody => this.Method == HttpMethod.Post || this.Method == HttpMethod.Put;

    public IReadOnlyList<string> ParameterNames => RouteValuePattern().Matches(this.PathTemplate)
                                                                       .Select(match => match.Groups[1].Value)
                                                                       .ToList();

    public string BuildPath(IReadOnlyDictionary<string, string> routeValues)
    {
        return RouteValuePattern().Replace(this.PathTemplate,
            match => Uri.EscapeDataString(routeValues.GetValueOrDefault(match.Groups[1].Value, string.Empty)));
    }

    [GeneratedRegex(@"\{(\w+)\}")]
    private static partial Regex RouteValuePattern();
}
