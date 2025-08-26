using SimRacingSdk.Acc.Core.Enums;

namespace SimRacingSdk.Acc.Core.Messages;

public record LogMessage(LoggingLevel Level, string Message, object? Data = null) { }