using SimRacingSdk.Acc.Udp.Enums;
using SimRacingSdk.Core;

namespace SimRacingSdk.Acc.Udp.Messages;

public class BroadcastingEvent
{
    public BroadcastingEventType BroadcastingEventType { get; internal set; }
    public CarInfo CarData { get; internal set; } = null!;
    public int CarId { get; internal set; }
    public string Message { get; internal set; } = null!;
    public int TimeMs { get; internal set; }

    public override string ToString()
    {
        return
            $"Broadcasting Event: Type: {this.BroadcastingEventType} Time: {this.TimeMs.ToTimingStringFromMilliseconds()} Message: {this.Message} Car Data: {this.CarData}";
    }
}