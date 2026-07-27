#nullable disable

using SimRacingSdk.Ace.Core.Enums;
using SimRacingSdk.Ace.Udp.Enums;

namespace SimRacingSdk.Ace.Udp.Messages;

public record DriverInfo
{
    public string FullName => $"{this.FirstName} {this.LastName}";
    public string InitialAndLastName => $"{this.FirstName[..1]}. {this.LastName}";
    public DriverCategory Category { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public AceNationality Nationality { get; init; }
    public string ShortName { get; init; }
}
