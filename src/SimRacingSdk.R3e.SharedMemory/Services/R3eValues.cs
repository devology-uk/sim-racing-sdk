using System.Text;

namespace SimRacingSdk.R3e.SharedMemory.Services;

// The game writes -1 for "not available" in almost every field; models expose those as null.
internal static class R3eValues
{
    private const float NotAvailable = -1f;
    private const double RadiansPerSecondToRpm = 60 / (2 * Math.PI);

    public static float? Available(float value)
    {
        return value == NotAvailable ? null : value;
    }

    public static int? Available(int value)
    {
        return value == -1 ? null : value;
    }

    public static bool? AvailableFlag(int value)
    {
        return value switch
        {
            0 => false,
            1 => true,
            _ => null
        };
    }

    public static string DecodeName(byte[]? utf8Bytes)
    {
        if(utf8Bytes is null)
        {
            return string.Empty;
        }

        var length = Array.IndexOf(utf8Bytes, (byte)0);
        return Encoding.UTF8.GetString(utf8Bytes, 0, length < 0 ? utf8Bytes.Length : length);
    }

    public static float? ToRpm(float radiansPerSecond)
    {
        return radiansPerSecond < 0 ? null : (float)(radiansPerSecond * RadiansPerSecondToRpm);
    }

    public static int? ToMilliseconds(float seconds)
    {
        return seconds <= 0 ? null : (int)Math.Round(seconds * 1000);
    }
}
