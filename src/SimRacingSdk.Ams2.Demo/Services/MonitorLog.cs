using Microsoft.Extensions.Logging;
using SimRacingSdk.Ams2.Demo.Abstractions;

namespace SimRacingSdk.Ams2.Demo.Services;

public class MonitorLog : IMonitorLog
{
    private readonly ILogger<MonitorLog> logger;

    public MonitorLog(ILogger<MonitorLog> logger)
    {
        this.logger = logger;
    }

    public void Log(string message)
    {
        this.logger.LogInformation(message);
    }
}