using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Models;

public record LmuIncidentStreamEvent(double EventTiming, string Message)
    : LmuStreamEvent(EventTiming, Message) { }