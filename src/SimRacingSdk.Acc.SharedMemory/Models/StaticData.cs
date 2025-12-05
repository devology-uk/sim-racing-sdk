#nullable disable

using SimRacingSdk.Acc.SharedMemory.Messages;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public record StaticData
{
    internal StaticData() {
        this.IsEmpty = true;
    }

    internal StaticData(StaticDataPage staticDataPage)
    {
        this.AccVersion = staticDataPage.AccVersion;
        this.AidAllowTyreBlankets = staticDataPage.AidAllowTyreBlankets;
        this.AidAutoBlip = staticDataPage.AidAutoBlip;
        this.AidAutoClutch = staticDataPage.AidAutoClutch;
        this.AidFuelRate = staticDataPage.AidFuelRate;
        this.AidMechanicalDamage = staticDataPage.AidMechanicalDamage;
        this.AidStability = staticDataPage.AidStability;
        this.AidTyreRate = staticDataPage.AidTireRate;
        this.CarModel = staticDataPage.CarModel;
        this.DryTyresName = staticDataPage.DryTyresName;
        this.IsOnline = staticDataPage.IsOnline;
        this.MaxFuel = staticDataPage.MaxFuel;
        this.MaxRpm = staticDataPage.MaxRpm;
        this.NumberOfCars = staticDataPage.NumberOfCars;
        this.NumberOfSessions = staticDataPage.NumberOfSessions;
        this.PenaltiesEnabled = staticDataPage.PenaltiesEnabled;
        this.PitWindowEnd = staticDataPage.PitWindowEnd;
        this.PitWindowStart = staticDataPage.PitWindowStart;
        this.PlayerFirstName = staticDataPage.PlayerFirstName;
        this.PlayerLastName = staticDataPage.PlayerSurname;
        this.PlayerNickname = staticDataPage.PlayerNickname;
        this.SectorCount = staticDataPage.SectorCount;
        this.SharedMemoryVersion = staticDataPage.SharedMemoryVersion;
        this.Track = staticDataPage.TrackName;
        this.WetTyresName = staticDataPage.WetTyresName;
    }

    public string AccVersion { get; }
    public bool AidAllowTyreBlankets { get; }
    public bool AidAutoBlip { get; }
    public bool AidAutoClutch { get; }
    public float AidFuelRate { get; }
    public float AidMechanicalDamage { get; }
    public float AidStability { get; }
    public float AidTyreRate { get; }
    public string CarModel { get; }
    public string DryTyresName { get; }
    public bool IsEmpty { get; }
    public bool IsOnline { get; }
    public float MaxFuel { get; }
    public int MaxRpm { get; }
    public int NumberOfCars { get; }
    public int NumberOfSessions { get; }
    public bool PenaltiesEnabled { get; }
    public int PitWindowEnd { get; }
    public int PitWindowStart { get; }
    public string PlayerFirstName { get; }
    public string PlayerLastName { get; }
    public string PlayerNickname { get; }
    public int SectorCount { get; }
    public string SharedMemoryVersion { get; }
    public string Track { get; }
    public string WetTyresName { get; }

    public string PlayerDisplayName()
    {
        if(!string.IsNullOrEmpty(this.PlayerFirstName))
        {
            return $"{this.PlayerFirstName[..1]}. {this.PlayerLastName}";
        }

        if(!string.IsNullOrEmpty(this.PlayerNickname))
        {
            return this.PlayerNickname;
        }

        return "Not Available";
    }

    internal bool IsActualEvent()
    {
        return !string.IsNullOrWhiteSpace(this.AccVersion);
    }
}