using SimRacingSdk.R3e.SharedMemory.Messages;

namespace SimRacingSdk.R3e.SharedMemory.Services;

internal sealed class R3eCarLapState
{
    public R3eCarLapState(R3eDriverData driver)
    {
        this.CompletedLaps = driver.CompletedLaps;
        this.CurrentLapValid = driver.CurrentLapValid;
        this.LastReportedLapTime = driver.SectorTimePreviousSelf.Sector3;
    }

    public int CompletedLaps { get; set; }
    public int CurrentLapValid { get; set; }
    public float LastReportedLapTime { get; set; }
    public R3ePendingLap? PendingLap { get; set; }
}
