#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record RaceSession
{
    [JsonIgnore]
    public int InvalidLaps
    {
        get
        {
            var carId = this.GetPlayerLeaderBoardLine()
                            .Car.CarId;
            return this.Laps.Count(l => l.CarId == carId && l.Flags != 0);
        }
    }

    [JsonIgnore]
    public int TotalLaps =>
        this.GetPlayerLeaderBoardLine()
            .Timing.LapCount;
    [JsonPropertyName("hasBeenSkipped")]
    public bool HasBeenSkipped { get; init; }
    [JsonPropertyName("laps")]
    public List<Lap> Laps { get; init; }
    [JsonPropertyName("sessionDef")]
    public SessionDef SessionDef { get; init; }
    [JsonPropertyName("snapShot")]
    public SnapShot SnapShot { get; init; }

    public Driver GetPlayer()
    {
        return this.SnapShot.GetPlayerLeaderBoardLine()
                   .Car.Drivers[0];
    }

    public LeaderBoardLine GetPlayerLeaderBoardLine()
    {
        return this.SnapShot.GetPlayerLeaderBoardLine();
    }

    public int GetPlayerFinishPosition()
    {
        var playerLeaderBoardLine = this.GetPlayerLeaderBoardLine();
        return this.SnapShot.LeaderBoardLines.IndexOf(playerLeaderBoardLine) + 1;
    }

    public Driver GetLapDriver(int carId, int driverId)
    {
        var carLeaderBoardLine = this.SnapShot.GetLeaderBoardLineByCarId(carId);
        return carLeaderBoardLine.Car.GetDriverByIndex(driverId);
    }
}