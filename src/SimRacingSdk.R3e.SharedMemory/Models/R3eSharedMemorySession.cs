using SimRacingSdk.R3e.SharedMemory.Enums;
using SimRacingSdk.R3e.SharedMemory.Messages;
using SimRacingSdk.R3e.SharedMemory.Services;

namespace SimRacingSdk.R3e.SharedMemory.Models;

public record R3eSharedMemorySession
{
    internal R3eSharedMemorySession(R3eShared data)
    {
        this.SessionId = Guid.NewGuid();
        this.GameMode = (R3eGameMode)data.GameMode;
        this.TrackId = data.TrackId;
        this.LayoutId = data.LayoutId;
        this.TrackName = R3eValues.DecodeName(data.TrackName);
        this.LayoutName = R3eValues.DecodeName(data.LayoutName);
        this.LayoutLengthMeters = data.LayoutLength;
        this.EventIndex = R3eValues.Available(data.EventIndex);
        this.SessionType = (R3eSessionType)data.SessionType;
        this.SessionIteration = R3eValues.Available(data.SessionIteration);
        this.SessionLengthFormat = (R3eSessionLengthFormat)data.SessionLengthFormat;
        this.NumberOfLaps = R3eValues.Available(data.NumberOfLaps);
        this.SessionTimeDurationSeconds = R3eValues.Available(data.SessionTimeDuration);
        this.NumberOfCars = Math.Clamp(data.NumCars, 0, R3eShared.MaxCars);
        this.PlayerUserId = R3eValues.Available(data.Player.UserId);
        this.PlayerName = R3eValues.DecodeName(data.PlayerName);
        this.IsRunning = true;
    }

    public int? EventIndex { get; }
    public R3eGameMode GameMode { get; }
    public bool IsRunning { get; internal set; }
    public int LayoutId { get; }
    public float LayoutLengthMeters { get; }
    public string LayoutName { get; }
    public int NumberOfCars { get; }
    public int? NumberOfLaps { get; }
    public string PlayerName { get; }
    public int? PlayerUserId { get; }
    public Guid SessionId { get; }
    public int? SessionIteration { get; }
    public R3eSessionLengthFormat SessionLengthFormat { get; }
    public float? SessionTimeDurationSeconds { get; }
    public R3eSessionType SessionType { get; }
    public int TrackId { get; }
    public string TrackName { get; }
}
