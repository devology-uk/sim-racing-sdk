#nullable disable

using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp.Messages;

public record VehicleClassUpdate : MessageBase
{
    public VehicleClassUpdate(MessageHeader header)
        : base(header) { }

    public VehicleClassInfo[] Classes { get; set; }
}