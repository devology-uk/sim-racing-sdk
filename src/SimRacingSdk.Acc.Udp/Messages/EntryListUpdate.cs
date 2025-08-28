namespace SimRacingSdk.Acc.Udp.Messages;

public class EntryListUpdate(string connectionIdentifier, CarInfo carInfo)
{
    public CarInfo CarInfo { get; } = carInfo;
    public string ConnectionIdentifier { get; } = connectionIdentifier;

    public override string ToString()
    {
        return $"Entry List Update: Connection: {this.ConnectionIdentifier}, Car Data: {this.CarInfo}";
    }
}