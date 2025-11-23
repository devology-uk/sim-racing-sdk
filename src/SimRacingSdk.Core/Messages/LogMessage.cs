using SimRacingSdk.Core.Enums;

namespace SimRacingSdk.Core.Messages;

public record LogMessage(LoggingLevel Level, string Content, string Source = "") { }