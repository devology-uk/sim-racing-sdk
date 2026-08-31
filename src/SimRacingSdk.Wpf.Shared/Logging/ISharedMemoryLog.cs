using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Wpf.Shared.Logging;

public interface ISharedMemoryLog
{
    void Log(LogMessage message);
}
