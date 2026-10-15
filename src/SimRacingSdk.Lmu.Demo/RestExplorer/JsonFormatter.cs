using System.Text.Json;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

public static class JsonFormatter
{
    private static readonly JsonSerializerOptions IndentedOptions = new() { WriteIndented = true };

    public static bool TryIndent(string text, out string indented)
    {
        try
        {
            using var document = JsonDocument.Parse(text);
            indented = JsonSerializer.Serialize(document.RootElement, IndentedOptions);
            return true;
        }
        catch(JsonException)
        {
            indented = text;
            return false;
        }
    }
}
