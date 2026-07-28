using Microsoft.Extensions.Logging;
using SimRacingSdk.Ace.Demo.Abstractions;

namespace SimRacingSdk.Ace.Demo.Services;

public class UdpLog : IUdpLog
{
    private readonly ILogger<UdpLog> logger;

    public UdpLog(ILogger<UdpLog> logger)
    {
        this.logger = logger;
    }

    public void Log(string message)
    {
        this.logger.LogInformation(message);
    }
}
