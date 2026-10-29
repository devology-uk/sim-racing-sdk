namespace SimRacingSdk.R3e.SharedMemory.Services;

internal sealed record R3ePendingLap(int LapValidity, DateTime CompletedAtUtc);
