namespace SimRacingSdk.Acc.Udp.Messages;

public class EntryListUpdate(string sender, CarInfo carInfo)
{
    public CarInfo CarInfo { get; } = carInfo;
    public string Sender { get; } = sender;

    public override string ToString()
    {
        return $"Entry List Update: Sender: {this.Sender}, Car Data: {this.CarInfo}";
    }
}