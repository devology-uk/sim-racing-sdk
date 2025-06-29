using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Models;

public record LmuTrackLimitsStreamEvent(
    double EventTiming,
    string Message,
    string Driver,
    int ID,
    int Lap,
    int WarningPoints,
    int CurrentPoints,
    int Resolution) : LmuStreamEvent(EventTiming, Message) { }