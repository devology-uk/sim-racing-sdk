#nullable disable

namespace SimRacingSdk.Ams2.Udp.Messages;

public class VehicleInfo
{
    public int Class { get; internal set; }
    public int Index { get; internal set; }
    public string Name { get; internal set; }
}