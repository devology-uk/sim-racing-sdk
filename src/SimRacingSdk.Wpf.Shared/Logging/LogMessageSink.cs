using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Wpf.Shared.Logging;

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
