using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Models;

public record LmuSectorStreamEvent(
    double EventTiming,
    string Message,
    string Driver,
    int Id,
    int Sector,
    string Class) : LmuStreamEvent(EventTiming, Message) { }