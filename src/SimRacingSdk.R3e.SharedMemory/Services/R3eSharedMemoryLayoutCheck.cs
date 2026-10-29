using System.Runtime.InteropServices;
using SimRacingSdk.R3e.SharedMemory.Messages;

namespace SimRacingSdk.R3e.SharedMemory.Services;

// The game publishes the size of its driver struct and the offset of NumCars, so comparing them with our own
// transcription catches any field-order or packing mistake on the first read instead of producing garbage.
internal static class R3eSharedMemoryLayoutCheck
{
    public const int SupportedVersionMajor = 3;
    public const int SupportedVersionMinor = 5;

    public static string? FindIncompatibility(R3eShared data)
    {
        if(data.VersionMajor != SupportedVersionMajor)
        {
            return $"R3E shared memory API version {data.VersionMajor}.{data.VersionMinor} is not supported "
                   + $"(expected {SupportedVersionMajor}.x).";
        }

        var driverDataSize = Marshal.SizeOf<R3eDriverData>();
        if(data.DriverDataSize != driverDataSize)
        {
            return $"R3E shared memory driver data is {data.DriverDataSize} bytes in the game "
                   + $"but {driverDataSize} bytes in the SDK.";
        }

        var allDriversOffset = Marshal.OffsetOf<R3eShared>(nameof(R3eShared.NumCars)).ToInt32();
        if(data.AllDriversOffset != allDriversOffset)
        {
            return $"R3E shared memory NumCars is at offset {data.AllDriversOffset} in the game "
                   + $"but {allDriversOffset} in the SDK.";
        }

        return null;
    }

    public static bool IsNewerMinorVersion(R3eShared data)
    {
        return data.VersionMinor > SupportedVersionMinor;
    }
}
