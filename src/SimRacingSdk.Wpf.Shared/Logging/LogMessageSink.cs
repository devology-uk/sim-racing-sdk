using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Wpf.Shared.Logging;

// Shared by ISharedMemoryLog/IMonitorLog/IUdpLog - each Demo app registers one instance per role, pointed at the
// role's own NLog category name (matching what nlog.config already routes), so this single class replaces what used
// to be three near-identical classes per game. Content is passed to ILogger's single-string overload, which (unlike
// the params-array overloads) never runs it through Microsoft.Extensions.Logging's template parser, so a record's
// ToString() dump (e.g. "GraphicsData { A = 1 }") reaches NLog's ${message} unmodified. Source rides on NLog's own
// ScopeContext (read back via ${scopeproperty:item=Source} in nlog.config) rather than the message text itself.
public class LogMessageSink : ISharedMemoryLog, IMonitorLog, IUdpLog
{
    private readonly ILogger logger;

    public LogMessageSink(ILogger logger)
    {
        this.logger = logger;
    }

    public void Log(LogMessage message)
    {
        using(NLog.ScopeContext.PushProperty("Source", message.Source))
        {
            this.logger.Log(ToLogLevel(message.Level), message.Content);
        }
    }

    private static LogLevel ToLogLevel(LoggingLevel level)
    {
        return level switch
        {
            LoggingLevel.Debug => LogLevel.Debug,
            LoggingLevel.Information => LogLevel.Information,
            LoggingLevel.Warning => LogLevel.Warning,
            LoggingLevel.Error => LogLevel.Error,
            _ => LogLevel.Information
        };
    }
}
