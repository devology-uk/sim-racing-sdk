#nullable disable
namespace SimRacingSdk.Acc.Udp.Messages;

public class TrackDataUpdate
{
    public Dictionary<string, List<string>> CameraSets { get; internal set; } = new();
    public IEnumerable<string> HudPages { get; internal set; } = new List<string>();
    public string ConnectionIdentifier { get; internal set; }
    public int TrackId { get; internal set; }
    public float TrackMeters { get; internal set; }
    public string TrackName { get; internal set; }
    public override string ToString()
    {
        return $"Track Data Update: Connection: {this.ConnectionIdentifier}, Track ID {this.TrackId}, Track: {this.TrackName}, Track Meters: {this.TrackMeters}";
    }
}