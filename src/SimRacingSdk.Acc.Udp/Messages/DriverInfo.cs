#nullable disable

using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Udp.Enums;

namespace SimRacingSdk.Acc.Udp.Messages;

public record DriverInfo
{
    public string FullName => $"{this.FirstName} {this.LastName}";
    public string InitialAndLastName => $"{this.FirstName[..1]}. {this.LastName}";
    public DriverCategory Category { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Nationality Nationality { get; init; }
    public string ShortName { get; init; }
}