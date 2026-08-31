using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Wpf.Shared.Logging;

public interface IMonitorLog
{
    void Log(LogMessage message);
}
