using SimRacingSdk.Ams2.Monitor.Enums;

namespace SimRacingSdk.Ams2.Monitor;

public static class Ams2MonitorExtensions
{
    public static string ToFriendlyName(this Ams2MonitorSessionType ams2MonitorSessionType)
    {
        return ams2MonitorSessionType switch
        {
            Ams2MonitorSessionType.TimeAttack => "Time Attack",
            _ => ams2MonitorSessionType.ToString()
        };
    }
}