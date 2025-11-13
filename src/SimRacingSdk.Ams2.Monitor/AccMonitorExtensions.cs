using SimRacingSdk.Ams2.Monitor.Enums;
using SimRacingSdk.Ams2.SharedMemory.Enums;

namespace SimRacingSdk.Ams2.Monitor;

internal static class AccMonitorExtensions
{
    internal static Ams2MonitorSessionType ToSessionType(this Ams2SessionState sessionState)
    {
        return sessionState switch
        {
            Ams2SessionState.Practice => Ams2MonitorSessionType.Practice,
            Ams2SessionState.Qualify => Ams2MonitorSessionType.Qualify,
            Ams2SessionState.Race => Ams2MonitorSessionType.Race,
            Ams2SessionState.Test => Ams2MonitorSessionType.Test,
            Ams2SessionState.TimeAttack => Ams2MonitorSessionType.TimeAttack,
            _ => Ams2MonitorSessionType.None
        };
    }
}