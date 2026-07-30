namespace SimRacingSdk.Ace.SharedMemory.Messages;

// Array-typed shared-memory fields need explicit formatting for logging - Array.ToString()
// only ever returns the type name, regardless of what FormatArray's own generated ToString does.
internal static class SharedMemoryLogFormatting
{
    public static string FormatArray<T>(T[] values)
    {
        return values == null ? "[]" : $"[{string.Join(", ", values)}]";
    }
}
