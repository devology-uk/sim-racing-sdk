using SimRacingSdk.R3e.SharedMemory.Messages;

namespace SimRacingSdk.R3e.SharedMemory.Services;

internal sealed record R3ePendingLap(R3eDriverData Driver, bool IsLocalPlayer, int LapValidity, DateTime CompletedAtUtc);
