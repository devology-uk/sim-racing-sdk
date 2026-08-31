using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Wpf.Shared.Logging;

public interface IUdpLog
{
    void Log(LogMessage message);
}
