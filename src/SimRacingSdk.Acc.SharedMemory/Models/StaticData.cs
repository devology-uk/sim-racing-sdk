using SimRacingSdk.Acc.SharedMemory.Messages;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public class StaticData(StaticDataPage staticDataPage)
{
    public string AccVersion { get; } = staticDataPage.AccVersion;

    public bool AidAllowTyreBlankets { get; } = staticDataPage.AidAllowTyreBlankets;

    public bool AidAutoBlip { get; } = staticDataPage.AidAutoBlip;

    public bool AidAutoClutch { get; } = staticDataPage.AidAutoClutch;

    public float AidFuelRate { get; } = staticDataPage.AidFuelRate;

    public float AidMechanicalDamage { get; } = staticDataPage.AidMechanicalDamage;

    public float AidStability { get; } = staticDataPage.AidStability;

    public float AidTyreRate { get; } = staticDataPage.AidTireRate;

    public string CarModel { get; } = staticDataPage.CarModel;

    public string DryTyresName { get; } = staticDataPage.DryTyresName;

    public bool IsOnline { get; } = staticDataPage.IsOnline;

    public float MaxFuel { get; } = staticDataPage.MaxFuel;

    public int MaxRpm { get; } = staticDataPage.MaxRpm;

    public int NumberOfCars { get; } = staticDataPage.NumberOfCars;

    public int NumberOfSessions { get; } = staticDataPage.NumberOfSessions;

    public bool PenaltiesEnabled { get; } = staticDataPage.PenaltiesEnabled;

    public int PitWindowEnd { get; } = staticDataPage.PitWindowEnd;

    public int PitWindowStart { get; } = staticDataPage.PitWindowStart;

    public string PlayerDisplayName() {
        
        if(!string.IsNullOrEmpty(this.PlayerName)) {
            return $"{this.PlayerName[..1]}. {this.PlayerSurname}";
        }

        if(!string.IsNullOrEmpty(this.PlayerNickname))
        {
            return this.PlayerNickname;
        }

        return "Not Available";
    }

    public string PlayerName { get; } = staticDataPage.PlayerFirstName;

    public string PlayerNickname { get; } = staticDataPage.PlayerNickname;

    public string PlayerSurname { get; } = staticDataPage.PlayerSurname;

    public int SectorCount { get; } = staticDataPage.SectorCount;

    public string SharedMemoryVersion { get; } = staticDataPage.SharedMemoryVersion;

    public string Track { get; } = staticDataPage.TrackName;

    public string WetTyresName { get; } = staticDataPage.WetTyresName;

    public override string ToString()
    {
        return
            $"Static Data Update: ACC Version: {this.AccVersion}, Track: {this.Track}, Car Model: {this.CarModel}, Driver: {this.PlayerDisplayName()}";
    }


    internal bool IsActualEvent()
    {
        return !string.IsNullOrWhiteSpace(this.AccVersion);
    }
}