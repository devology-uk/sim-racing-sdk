#nullable disable

using SimRacingSdk.Ace.SharedMemory.Enums;
using SimRacingSdk.Ace.SharedMemory.Messages;

namespace SimRacingSdk.Ace.SharedMemory.Models;

public record AceStaticData
{
    private readonly AceStaticDataPage staticDataPage;

    internal AceStaticData()
    {
        this.IsConnected = false;
    }

    internal AceStaticData(AceStaticDataPage staticDataPage)
    {
        this.staticDataPage = staticDataPage;
        this.SharedMemoryVersion = staticDataPage.SharedMemoryVersion;
        this.AceEvoVersion = staticDataPage.AceEvoVersion;
        this.Session = staticDataPage.Session;
        this.SessionName = staticDataPage.SessionName;
        this.EventId = staticDataPage.EventId;
        this.SessionId = staticDataPage.SessionId;
        this.StartingGrip = staticDataPage.StartingGrip;
        this.StartingAmbientTemperatureC = staticDataPage.StartingAmbientTemperatureC;
        this.StartingGroundTemperatureC = staticDataPage.StartingGroundTemperatureC;
        this.IsStaticWeather = staticDataPage.IsStaticWeather;
        this.IsTimedRace = staticDataPage.IsTimedRace;
        this.IsOnline = staticDataPage.IsOnline;
        this.NumberOfSessions = staticDataPage.NumberOfSessions;
        this.Nation = staticDataPage.Nation;
        this.Longitude = staticDataPage.Longitude;
        this.Latitude = staticDataPage.Latitude;
        this.Track = staticDataPage.Track;
        this.TrackConfiguration = staticDataPage.TrackConfiguration;
        this.TrackLengthM = staticDataPage.TrackLengthM;
        this.IsConnected = true;
    }

    public string SharedMemoryVersion { get; }
    public string AceEvoVersion { get; }
    public AceSessionType Session { get; }
    public string SessionName { get; }
    public byte EventId { get; }
    public byte SessionId { get; }
    public AceStartingGrip StartingGrip { get; }
    public float StartingAmbientTemperatureC { get; }
    public float StartingGroundTemperatureC { get; }
    public bool IsStaticWeather { get; }
    public bool IsTimedRace { get; }
    public bool IsOnline { get; }
    public int NumberOfSessions { get; }
    public string Nation { get; }
    public float Longitude { get; }
    public float Latitude { get; }
    public string Track { get; }
    public string TrackConfiguration { get; }
    public float TrackLengthM { get; }
    public bool IsConnected { get; }

    // Evo's static page is written once per session/event and carries EventId/SessionId as its
    // stable identity - unlike Acc, where session identity lives on the per-frame graphics page.
    public bool IsSameSession(AceStaticData other)
    {
        return other != null && this.EventId == other.EventId && this.SessionId == other.SessionId
                              && this.Session == other.Session && this.IsOnline == other.IsOnline;
    }
}
