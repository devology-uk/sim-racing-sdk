using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Models;

public record LmuScoreStreamEvent(double EventTiming, string Message)
    : LmuStreamEvent(EventTiming, Message) { }