using SimRacingSdk.Acc.Udp.Enums;
using SimRacingSdk.Core;

namespace SimRacingSdk.Acc.Udp.Messages;

public class LapInfo
{
    public List<int?> Splits { get; } = new();
    public ushort CarIndex { get; internal set; }
    public ushort DriverIndex { get; internal set; }
    public bool IsInvalid { get; internal set; }
    public bool IsValidForBest { get; internal set; }
    public int? LapTimeMs { get; set; }
    public LapType LapType { get; internal set; }

    public byte SplitCount { get; set; }

    public override string ToString()
    {
        return
            $"Lap Time: {this.LapTimeMs.GetValueOrDefault().ToTimingStringFromMilliseconds()} Splits: [{this.Splits[0].GetValueOrDefault().ToTimingStringFromMilliseconds()},{this.Splits[1].GetValueOrDefault().ToTimingStringFromMilliseconds()},{this.Splits[2].GetValueOrDefault().ToTimingStringFromMilliseconds()}] Invalid: {this.IsInvalid}, Lap Type: {this.LapType}";
    }

    internal void UpdateFromReader(BinaryReader binaryReader)
    {
        this.LapTimeMs = binaryReader.ReadInt32();
        this.CarIndex = binaryReader.ReadUInt16();
        this.DriverIndex = binaryReader.ReadUInt16();
        this.SplitCount = binaryReader.ReadByte();
        for(var i = 0; i < this.SplitCount; i++)
        {
            this.Splits.Add(binaryReader.ReadInt32());
        }

        this.IsInvalid = binaryReader.ReadByte() > 0;
        this.IsValidForBest = binaryReader.ReadByte() > 0;

        var isOutLap = binaryReader.ReadByte() > 0;
        var isInLap = binaryReader.ReadByte() > 0;

        if(isOutLap)
        {
            this.LapType = LapType.Outlap;
        }
        else if(isInLap)
        {
            this.LapType = LapType.Inlap;
        }
        else
        {
            this.LapType = LapType.Regular;
        }

        while(this.Splits.Count < 3)
        {
            this.Splits.Add(null);
        }

        for(var i = 0; i < this.Splits.Count; i++)
        {
            if(this.Splits[i] == int.MaxValue)
            {
                this.Splits[i] = 0;
            }
        }

        if(this.LapTimeMs == int.MaxValue)
        {
            this.LapTimeMs = 0;
        }
    }
}