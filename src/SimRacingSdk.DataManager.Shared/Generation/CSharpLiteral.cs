using System.Globalization;

namespace SimRacingSdk.DataManager.Shared.Generation;

// Formats captured values as C# source for the Core provider generators.
public static class CSharpLiteral
{
    public static string Bool(bool value)
    {
        return value ? "true" : "false";
    }

    public static string NullableNumber(double? value)
    {
        return value.HasValue ? Number(value.Value) : "null";
    }

    public static string NullableText(string? value)
    {
        return value is null ? "null" : Text(value);
    }

    public static string Number(double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static string Text(string value)
    {
        return $"\"{value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
    }

    public static string TextArray(IEnumerable<string> values)
    {
        return $"[{string.Join(", ", values.Select(Text))}]";
    }
}
