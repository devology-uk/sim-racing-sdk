#nullable disable

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccDriverEntry(
    string FirstName,
    string LastName,
    string ShortName,
    string Category,
    string Nationality)
{
    public string FullName => $"{this.FirstName} {this.LastName}";
    public string InitialAndLastName => $"{this.FirstName[..1]}. {this.LastName}";
}