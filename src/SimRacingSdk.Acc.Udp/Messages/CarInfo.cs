#nullable disable

using SimRacingSdk.Acc.Core.Enums;

namespace SimRacingSdk.Acc.Udp.Messages;

public class CarInfo(ushort carIndex)
{
    public ushort CarIndex { get; } = carIndex;
    public IList<DriverInfo> Drivers { get; } = new List<DriverInfo>();
    public byte CarModelType { get; internal set; }
    public byte CupCategory { get; internal set; }
    public int CurrentDriverIndex { get; internal set; }
    public AccNationality Nationality { get; internal set; }
    public int RaceNumber { get; internal set; }
    public string TeamName { get; internal set; }

    public string GetCurrentDriverDisplayName()
    {
        return this.CurrentDriverIndex >= this.Drivers.Count
                   ? "nobody(?)"
                   : this.Drivers[this.CurrentDriverIndex].InitialAndLastName;
    }

    public override string ToString()
    {
        return !this.Drivers.Any()
                   ? $"Race Number: {this.RaceNumber}, Team: {this.TeamName}"
                   : $"Race Number: {this.RaceNumber}, Driver: {this.Drivers[this.CurrentDriverIndex].InitialAndLastName}, Car: {this.CarModelType}";
    }

    internal void AddDriver(DriverInfo driverInfo)
    {
        this.Drivers.Add(driverInfo);
    }
}