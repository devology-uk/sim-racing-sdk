using SimRacingSdk.R3e.SharedMemory.Messages;
using SimRacingSdk.R3e.SharedMemory.Models;

namespace SimRacingSdk.R3e.SharedMemory.Services;

// Watches every car's CompletedLaps. A lap's validity is the last CurrentLapValid seen before the counter rolled
// over; its time comes from SectorTimePreviousSelf, which may not update on the same tick as the counter, so the
// lap waits for a new previous-lap time and is sent without one if none arrives within the grace period (an
// invalidated lap may never get one).
internal sealed class R3eLapTracker
{
    private static readonly TimeSpan LapTimeGracePeriod = TimeSpan.FromSeconds(2);

    private readonly Dictionary<int, R3eCarLapState> carStates = new();

    public void Reset()
    {
        this.carStates.Clear();
    }

    // The game goes to the results menu as the player crosses the line at the end of a session, so the final
    // lap can still be waiting for its time when the session ends.
    public IReadOnlyList<R3eSharedMemoryLap> FlushPendingLaps(Guid sessionId)
    {
        var pendingLaps = this.carStates.Values
                              .Select(state => state.PendingLap)
                              .OfType<R3ePendingLap>()
                              .Select(pendingLap => new R3eSharedMemoryLap(pendingLap.Driver, null, pendingLap.LapValidity,
                                  sessionId, pendingLap.IsLocalPlayer))
                              .ToList();
        this.Reset();
        return pendingLaps;
    }

    public IReadOnlyList<R3eSharedMemoryLap> Update(R3eShared data, Guid sessionId)
    {
        var completedLaps = new List<R3eSharedMemoryLap>();
        var numberOfCars = Math.Clamp(data.NumCars, 0, R3eShared.MaxCars);
        var nowUtc = DateTime.UtcNow;
        for(var index = 0; index < numberOfCars; index++)
        {
            var driver = data.DriverData[index];
            var isLocalPlayer = driver.DriverInfo.SlotId == data.VehicleInfo.SlotId;
            var lap = this.UpdateCar(driver, sessionId, isLocalPlayer, nowUtc);
            if(lap is not null)
            {
                completedLaps.Add(lap);
            }
        }

        return completedLaps;
    }

    private static bool HasNewLapTime(R3eDriverData driver, R3eCarLapState state)
    {
        var lapTime = driver.SectorTimePreviousSelf.Sector3;
        return lapTime > 0 && lapTime != state.LastReportedLapTime;
    }

    private R3eSharedMemoryLap? CompletePendingLap(R3eDriverData driver, R3eCarLapState state, Guid sessionId,
        bool isLocalPlayer, DateTime nowUtc)
    {
        if(state.PendingLap is not { } pendingLap)
        {
            return null;
        }

        if(HasNewLapTime(driver, state))
        {
            state.PendingLap = null;
            state.LastReportedLapTime = driver.SectorTimePreviousSelf.Sector3;
            return new R3eSharedMemoryLap(driver, driver.SectorTimePreviousSelf, pendingLap.LapValidity, sessionId,
                isLocalPlayer);
        }

        if(nowUtc - pendingLap.CompletedAtUtc < LapTimeGracePeriod)
        {
            return null;
        }

        state.PendingLap = null;
        return new R3eSharedMemoryLap(pendingLap.Driver, null, pendingLap.LapValidity, sessionId, isLocalPlayer);
    }

    private R3eSharedMemoryLap? UpdateCar(R3eDriverData driver, Guid sessionId, bool isLocalPlayer,
        DateTime nowUtc)
    {
        var slotId = driver.DriverInfo.SlotId;
        if(slotId < 0 || driver.CompletedLaps < 0)
        {
            return null;
        }

        if(!this.carStates.TryGetValue(slotId, out var state) || driver.CompletedLaps < state.CompletedLaps)
        {
            this.carStates[slotId] = new R3eCarLapState(driver);
            return null;
        }

        if(driver.CompletedLaps > state.CompletedLaps)
        {
            state.PendingLap = new R3ePendingLap(driver, isLocalPlayer, state.CurrentLapValid, nowUtc);
        }

        state.CompletedLaps = driver.CompletedLaps;
        state.CurrentLapValid = driver.CurrentLapValid;
        return this.CompletePendingLap(driver, state, sessionId, isLocalPlayer, nowUtc);
    }
}
