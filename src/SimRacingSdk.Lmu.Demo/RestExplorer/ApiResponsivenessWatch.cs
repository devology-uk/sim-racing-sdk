using SimRacingSdk.GameControl.Abstractions;
using SimRacingSdk.Lmu.Core.Services;
using SimRacingSdk.Lmu.Rest.Abstractions;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

// Polls one cheap endpoint every second and reports only changes - API responding or not, and LMU's window
// going active, background or minimised - so cause and effect line up in the log.
public class ApiResponsivenessWatch
{
    public const string WatchedPath = "/rest/sessions/GetGameState";

    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(1);

    private readonly IGameWindowFinder gameWindowFinder;
    private readonly IGameWindowStateReader gameWindowStateReader;

    public ApiResponsivenessWatch(IGameWindowFinder gameWindowFinder, IGameWindowStateReader gameWindowStateReader)
    {
        this.gameWindowFinder = gameWindowFinder;
        this.gameWindowStateReader = gameWindowStateReader;
    }

    public async Task RunAsync(ILmuRestClient client, Action<string> report, CancellationToken cancellationToken)
    {
        bool? wasResponding = null;
        string? previousWindowState = null;
        while(!cancellationToken.IsCancellationRequested)
        {
            var windowState = this.ReadWindowState();
            if(windowState != previousWindowState)
            {
                report($"{DateTime.Now:HH:mm:ss.fff} LMU window: {windowState}");
                previousWindowState = windowState;
            }

            var response = await client.SendAsync(HttpMethod.Get, WatchedPath, cancellationToken: cancellationToken);
            if(cancellationToken.IsCancellationRequested)
            {
                return;
            }

            if(response.IsResponding != wasResponding)
            {
                report($"{DateTime.Now:HH:mm:ss.fff} API {(response.IsResponding ? "responding" : "NOT responding")} "
                    + $"(window {windowState}): {ResponseSummary.Describe(response)}");
                wasResponding = response.IsResponding;
            }

            await DelayUnlessCancelled(cancellationToken);
        }
    }

    private static async Task DelayUnlessCancelled(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(PollInterval, cancellationToken);
        }
        catch(TaskCanceledException)
        {
        }
    }

    private string ReadWindowState()
    {
        var gameWindow = this.gameWindowFinder.Find(LmuGameDetector.ProcessName);
        return gameWindow == null ? "not found" : this.gameWindowStateReader.Read(gameWindow).ToString();
    }
}
