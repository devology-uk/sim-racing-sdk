#nullable disable

using SimRacingSdk.Pmr.Monitor.Abstractions;

namespace SimRacingSdk.Pmr.Monitor.Messages;

public record PmrMonitorLap : PmrMonitorMessageBase
{
    public int CompletedLap { get; init; }
    public string DriverName { get; init; }
    public bool IsDisqualified { get; init; }

    // Not yet rig-confirmed as reliable for a human-driven car - a full AI-only race weekend
    // showed this true for every car for the whole session. Passed through raw rather than
    // second-guessed here; picking out "the player" is left to the consuming app.
    public bool IsPlayer { get; init; }

    // The UDP protocol has no explicit "last lap time" field - this is the participant's own
    // CurrentLapTimeSeconds/CurrentSectorTimesSeconds as last received before CurrentLap ticked
    // over, so it can be up to one UDP interval short of the true final time.
    public double LapTimeSeconds { get; init; }
    public int Position { get; init; }
    public IReadOnlyList<double> SectorTimesSeconds { get; init; } = [];
    public Guid SessionId { get; init; }
    public string VehicleClass { get; init; }
    public int VehicleId { get; init; }
    public string VehicleName { get; init; }
}
