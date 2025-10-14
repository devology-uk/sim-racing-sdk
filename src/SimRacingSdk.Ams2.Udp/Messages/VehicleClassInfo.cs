#nullable disable

namespace SimRacingSdk.Ams2.Udp.Messages;

public record VehicleClassInfo
{
    public int Index { get; internal set; }
    public string Name { get; internal set; }
}