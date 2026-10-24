using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.Rest.Abstractions;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Rest;

// Polls LMU's REST API in the background, one request at a time. Session info and game state are read every
// poll; standings only while on track; the slower-changing reads every few polls. Each read is ~30 ms on the
// game's side (rig captures 2026-10-24).
public class LmuRestConnection : ILmuRestConnection
{
    private const int SlowReadEveryNthPoll = 5;

    private static readonly TimeSpan DefaultPollInterval = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan UnavailableRetryInterval = TimeSpan.FromSeconds(5);

    private readonly Subject<bool> availabilitySubject = new();
    private readonly ILmuRestClient client;
    private readonly LogMessageBroker logMessageBroker = new(nameof(LmuRestConnection));
    private readonly Subject<LmuRestPitStopEstimate> pitStopEstimatesSubject = new();
    private readonly Subject<LmuRestResponse> responsesSubject = new();
    private readonly Subject<IReadOnlyList<LmuRestStanding>> standingsSubject = new();
    private readonly Subject<LmuRestStatus> statusSubject = new();
    private readonly Subject<IReadOnlyDictionary<string, IReadOnlyList<LmuRestStintUsage>>> strategyUsageSubject = new();
    private readonly Subject<LmuRestVehicleCondition> vehicleConditionsSubject = new();

    private CancellationTokenSource? pollCancellation;
    private bool? wasAvailable;

    public LmuRestConnection(ILmuRestClient client)
    {
        this.client = client;
    }

    public IObservable<bool> Availability => this.availabilitySubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    public IObservable<LmuRestPitStopEstimate> PitStopEstimates => this.pitStopEstimatesSubject.AsObservable();
    public IObservable<LmuRestResponse> Responses => this.responsesSubject.AsObservable();
    public IObservable<IReadOnlyList<LmuRestStanding>> Standings => this.standingsSubject.AsObservable();
    public IObservable<LmuRestStatus> Status => this.statusSubject.AsObservable();

    public IObservable<IReadOnlyDictionary<string, IReadOnlyList<LmuRestStintUsage>>> StrategyUsage =>
        this.strategyUsageSubject.AsObservable();

    public IObservable<LmuRestVehicleCondition> VehicleConditions => this.vehicleConditionsSubject.AsObservable();

    public void Dispose()
    {
        this.Stop();
        this.client.Dispose();
        GC.SuppressFinalize(this);
    }

    public void Start(TimeSpan? pollInterval = null)
    {
        this.Stop();
        this.LogMessage(LoggingLevel.Information, $"Starting LMU REST connection to {this.client.BaseAddress}...");
        this.pollCancellation = new CancellationTokenSource();
        var cancellationToken = this.pollCancellation.Token;
        Task.Run(() => this.PollUntilCancelled(pollInterval ?? DefaultPollInterval, cancellationToken),
            cancellationToken);
    }

    public void Stop()
    {
        this.pollCancellation?.Cancel();
        this.pollCancellation?.Dispose();
        this.pollCancellation = null;
        this.wasAvailable = null;
    }

    private static async Task DelayUnlessCancelled(TimeSpan delay, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(delay, cancellationToken);
        }
        catch(TaskCanceledException)
        {
        }
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessageBroker.Log(level, content);
    }

    private async Task PollUntilCancelled(TimeSpan pollInterval, CancellationToken cancellationToken)
    {
        try
        {
            var pollCount = 0L;
            while(!cancellationToken.IsCancellationRequested)
            {
                var status = await this.ReadStatus(cancellationToken);
                if(status == null)
                {
                    await DelayUnlessCancelled(UnavailableRetryInterval, cancellationToken);
                    continue;
                }

                await this.ReadOnTrackData(status, pollCount % SlowReadEveryNthPoll == 0, cancellationToken);
                pollCount++;
                await DelayUnlessCancelled(pollInterval, cancellationToken);
            }
        }
        catch(Exception exception)
        {
            this.LogMessage(LoggingLevel.Error, $"LMU REST polling stopped: {exception.Message}");
        }
    }

    private async Task<LmuRestStatus?> ReadStatus(CancellationToken cancellationToken)
    {
        var sessionInfoResponse = await this.Send(LmuRestPaths.SessionInfo, cancellationToken);
        this.UpdateAvailability(sessionInfoResponse.IsResponding);
        if(!sessionInfoResponse.IsResponding)
        {
            return null;
        }

        var gameStateResponse = await this.Send(LmuRestPaths.GameState, cancellationToken);
        var status = new LmuRestStatus(LmuRestJson.TryRead<LmuRestSessionInfo>(sessionInfoResponse),
            LmuRestJson.TryRead<LmuRestGameState>(gameStateResponse));
        this.statusSubject.OnNext(status);
        return status;
    }

    // The player-vehicle read goes first, straight after the status that allowed it, so the game has the least
    // time to leave the session in between.
    private async Task ReadOnTrackData(LmuRestStatus status, bool isSlowReadDue, CancellationToken cancellationToken)
    {
        if(!status.IsInRealtime)
        {
            return;
        }

        if(isSlowReadDue && status.CanReadPlayerVehicle)
        {
            await this.ReadAndPublish(LmuRestPaths.VehicleCondition, this.vehicleConditionsSubject, cancellationToken);
        }

        await this.ReadAndPublish(LmuRestPaths.Standings, this.standingsSubject, cancellationToken);
        if(isSlowReadDue)
        {
            await this.ReadAndPublish(LmuRestPaths.PitStopEstimate, this.pitStopEstimatesSubject, cancellationToken);
            await this.ReadAndPublish(LmuRestPaths.StrategyUsage, this.strategyUsageSubject, cancellationToken);
        }
    }

    private async Task ReadAndPublish<T>(string path, Subject<T> subject, CancellationToken cancellationToken)
        where T : class
    {
        var value = LmuRestJson.TryRead<T>(await this.Send(path, cancellationToken));
        if(value != null)
        {
            subject.OnNext(value);
        }
    }

    private async Task<LmuRestResponse> Send(string path, CancellationToken cancellationToken)
    {
        var response = await this.client.SendAsync(HttpMethod.Get, path, cancellationToken: cancellationToken);
        this.responsesSubject.OnNext(response);
        return response;
    }

    private void UpdateAvailability(bool isAvailable)
    {
        if(isAvailable == this.wasAvailable)
        {
            return;
        }

        this.wasAvailable = isAvailable;
        this.LogMessage(LoggingLevel.Information, isAvailable ? "LMU REST API responding" : "LMU REST API not responding");
        this.availabilitySubject.OnNext(isAvailable);
    }
}
