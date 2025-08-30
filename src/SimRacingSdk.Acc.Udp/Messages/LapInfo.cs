#nullable disable

using SimRacingSdk.Acc.Udp.Enums;

namespace SimRacingSdk.Acc.Udp.Messages;

public record LapInfo
{
    public ushort CarIndex { get; internal set; }
    public ushort DriverIndex { get; internal set; }
    public bool IsInvalid { get; internal set; }
    public bool IsValidForBest { get; internal set; }
    public int? LapTimeMs { get; set; }
    public LapType LapType { get; internal set; }
    public byte SplitCount { get; set; }
    public List<int?> Splits { get; internal set; }
}