using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Rest.Abstractions;

public interface ILmuRestConnection : IDisposable
{
    IObservable<bool> Availability { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<LmuRestPitStopEstimate> PitStopEstimates { get; }
    IObservable<LmuRestResponse> Responses { get; }
    IObservable<IReadOnlyList<LmuRestStanding>> Standings { get; }
    IObservable<LmuRestStatus> Status { get; }
    IObservable<IReadOnlyDictionary<string, IReadOnlyList<LmuRestStintUsage>>> StrategyUsage { get; }
    IObservable<LmuRestVehicleCondition> VehicleConditions { get; }
    void Start(TimeSpan? pollInterval = null);
    void Stop();
}
