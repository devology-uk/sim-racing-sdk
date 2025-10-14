#nullable disable

using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp.Messages;

public record VehicleInfoUpdate : MessageBase
{
    public VehicleInfoUpdate(MessageHeader header)
        : base(header) { }

    public VehicleInfo[] Vehicles { get; internal set; }
}