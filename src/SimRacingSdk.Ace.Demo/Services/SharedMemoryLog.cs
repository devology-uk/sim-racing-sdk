using Microsoft.Extensions.Logging;
using SimRacingSdk.Ace.Demo.Abstractions;

namespace SimRacingSdk.Ace.Demo.Services;

public class SharedMemoryLog : ISharedMemoryLog
{
    private readonly ILogger<SharedMemoryLog> logger;

    public SharedMemoryLog(ILogger<SharedMemoryLog> logger)
    {
        this.logger = logger;
    }

    public void Log(string message)
    {
        this.logger.LogInformation(message);
    }
}
