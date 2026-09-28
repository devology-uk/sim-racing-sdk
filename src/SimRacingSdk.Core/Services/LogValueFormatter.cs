using System.Collections;
using System.Reflection;

namespace SimRacingSdk.Core.Services;

// Compiler-generated record and struct ToString output prints arrays and lists as their type name, which hides
// per-wheel telemetry in logs; this walks SDK types' public members and expands sequences instead.
public static class LogValueFormatter
{
    private const int MaxDepth = 5;
    private const string SdkNamespace = "SimRacingSdk";

    public static string Format(object? value)
    {
        return FormatValue(value, 0);
    }

    public static string FormatMembers(object value)
    {
        return FormatMembers(value, 0);
    }

    private static string FormatValue(object? value, int depth)
    {
        return value switch
        {
            null => "null",
            string text => text,
            Enum enumValue => enumValue.ToString(),
            IEnumerable sequence => FormatSequence(sequence, depth),
            _ when IsSdkType(value.GetType()) && depth < MaxDepth =>
                $"{value.GetType().Name} {{ {FormatMembers(value, depth + 1)} }}",
            _ => value.ToString() ?? string.Empty
        };
    }

    private static string FormatSequence(IEnumerable sequence, int depth)
    {
        var items = sequence.Cast<object?>().Select(item => FormatValue(item, depth + 1));
        return $"[{string.Join(", ", items)}]";
    }

    private static string FormatMembers(object value, int depth)
    {
        var type = value.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(property => property.CanRead && property.GetIndexParameters().Length == 0)
                             .Select(property => FormatMember(property.Name, property.GetValue(value), depth));
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                         .Select(field => FormatMember(field.Name, field.GetValue(value), depth));
        return string.Join(", ", properties.Concat(fields));
    }

    private static string FormatMember(string name, object? value, int depth)
    {
        return $"{name} = {FormatValue(value, depth)}";
    }

    private static bool IsSdkType(Type type)
    {
        return type.Namespace?.StartsWith(SdkNamespace, StringComparison.Ordinal) == true;
    }
}
