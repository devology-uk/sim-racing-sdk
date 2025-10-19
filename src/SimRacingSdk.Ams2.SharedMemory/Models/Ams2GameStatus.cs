#nullable disable

using SimRacingSdk.Ams2.SharedMemory.Enums;

namespace SimRacingSdk.Ams2.SharedMemory.Models;

public record Ams2GameStatus
{
    public float AmbientTempC { get; init; }
    public uint BuildVersion { get; init; }
    public float CloudBrightness { get; init; }
    public uint DPadMask { get; init; }
    public int FocusedParticipantIndex { get; init; }
    public Ams2GameState GameState { get; init; }
    public uint JoyPad0Mask { get; init; }
    public uint LapsInEvent { get; init; }
    public int ParticipantCount { get; init; }
    public Ams2RaceState RaceState { get; init; }
    public float RainDensity { get; init; }
    public int SectorCount { get; init; }
    public int SessionAdditionalLaps { get; init; }
    public float SessionDuration { get; init; }
    public Ams2SessionState SessionState { get; init; }
    public float SnowDensity { get; init; }
    public string TrackLayout { get; init; }
    public float TrackLength { get; init; }
    public string TrackLocation { get; init; }
    public float TrackTempC { get; init; }
    public uint Version { get; init; }
    public float WindDirectionX { get; init; }
    public float WindDirectionY { get; init; }
    public float WindSpeed { get; init; }
}