using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Models;

public record LmuSentStreamEvent(double EventTiming, string Message)
    : LmuStreamEvent(EventTiming, Message) { }