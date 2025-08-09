using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Udp.Enums;

namespace SimRacingSdk.Acc.Udp.Messages;

public struct DriverInfo
{
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public string ShortName { get; internal set; }
    public DriverCategory Category { get; internal set; }
    public Nationality Nationality { get; internal set; }

    public override string ToString()
    {
        return $"Driver: {this.FirstName} {this.LastName} ({this.ShortName}) Category: {this.Category}";
    }

    public string InitialAndLastName => $"{this.FirstName[..1]}. {this.LastName}";
    public string FullName => $"{this.FirstName} {this.LastName}";
}