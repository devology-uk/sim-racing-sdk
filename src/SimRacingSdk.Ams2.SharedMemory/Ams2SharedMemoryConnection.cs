using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;
using SimRacingSdk.Ams2.SharedMemory.Models;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.SharedMemory;

public class Ams2SharedMemoryConnection : IAms2SharedMemoryConnection
{
    private readonly IAms2SharedMemoryProvider ams2SharedMemoryProvider;
    private readonly Subject<Ams2Lap> completedLapsSubject = new();
    private readonly Dictionary<int, Ams2Participant> entries = [];
    private readonly Subject<Ams2GameStatus> gameStatusUpdatesSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<Ams2Participant> participantUpdatesSubject = new();
    private readonly Subject<Ams2TelemetryFrame> telemetrySubject = new();

    private Ams2GameStatus? currentGameStatus;
    private double updateIntervalMs;
    private IDisposable? updateSubscription;

    public Ams2SharedMemoryConnection(IAms2SharedMemoryProvider ams2SharedMemoryProvider)
    {
        this.ams2SharedMemoryProvider = ams2SharedMemoryProvider;
    }

    public IObservable<Ams2Lap> CompletedLaps => this.completedLapsSubject.AsObservable();
    public IObservable<Ams2GameStatus> GameStatusUpdates => this.gameStatusUpdatesSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<Ams2Participant> ParticipantUpdates => this.participantUpdatesSubject.AsObservable();
    public IObservable<Ams2TelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(double updateIntervalMs = 300)
    {
        this.updateIntervalMs = updateIntervalMs;
        this.updateSubscription = Observable.Interval(TimeSpan.FromMilliseconds(updateIntervalMs))
                                            .Subscribe(this.OnNextUpdate, this.OnError, this.OnCompleted);
    }

    public void Stop()
    {
        this.updateSubscription?.Dispose();
        this.updateSubscription = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.updateSubscription?.Dispose();
    }

    private void LogMessage(LoggingLevel loggingLevel, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel, content));
    }

    private void OnCompleted()
    {
        this.LogMessage(LoggingLevel.Warning, "Update stream completed.");
    }

    private void OnError(Exception exception)
    {
        this.LogMessage(LoggingLevel.Error,
            $"Unexpected error processing updates: {exception.GetBaseException().Message}");
    }

    private void OnNextUpdate(long index)
    {
        var sharedMemoryData = this.ams2SharedMemoryProvider.ReadSharedMemoryData();

        if(sharedMemoryData.IsEmpty || sharedMemoryData.SequenceNumber % 2 > 0)
        {
            Debug.WriteLine("Received incomplete data page");
            return;
        }

        this.UpdateGameStatus(sharedMemoryData);
        this.UpdateTelemetry(sharedMemoryData);
        this.UpdateParticipants(sharedMemoryData);
    }

    private void UpdateGameStatus(SharedMemoryData sharedMemoryData)
    {
        var gameStatus = sharedMemoryData.GetGameStatus();
       
        if(this.currentGameStatus != null && this.currentGameStatus.GameState == gameStatus.GameState
                                          && this.currentGameStatus.SessionState == gameStatus.SessionState
                                          && this.currentGameStatus.RaceState == gameStatus.RaceState)
        {
            return;
        }

        this.gameStatusUpdatesSubject.OnNext(gameStatus);
        this.LogMessage(LoggingLevel.Information, $"Game State Update: {gameStatus}");
        this.currentGameStatus = gameStatus;

    }

    private void UpdateParticipants(SharedMemoryData sharedMemoryData)
    {
        var participants = sharedMemoryData.GetParticipants();
        foreach(var participant in participants)
        {
            this.LogMessage(LoggingLevel.Information, $"Participant Update: {participant}");
            if(this.entries.TryGetValue(participant.Index, out var entry))
            {
                if(participant.LapsCompleted > entry.LapsCompleted)
                {
                    var completedLap = new Ams2Lap
                    {
                        BestSector1Time = participant.BestSector1Time,
                        BestSector2Time = participant.BestSector2Time,
                        BestSector3Time = participant.BestSector3Time,
                        CarClassName = entry.CarClassName,
                        CarName = entry.CarName,
                        IsPlayerLap = entry.IsFocusedParticipant,
                        IsValid = !entry.IsLapInvalid,
                        Lap = entry.CurrentLap,
                        LapTime = participant.LastLapTime,
                        ParticipantIndex = participant.Index,
                        ParticipantName = entry.Name,
                        ParticipantNationality = entry.Nationality,
                        Sector1Time = entry.CurrentSector1Time,
                        Sector2Time = entry.CurrentSector2Time,
                        Sector3Time = entry.CurrentSector3Time
                                      + TimeSpan.FromMilliseconds(
                                          this.updateIntervalMs
                                          - participant.CurrentSector1Time.TotalMilliseconds)
                    };

                    this.LogMessage(LoggingLevel.Information, $"Completed Lap: {completedLap}");
                    this.completedLapsSubject.OnNext(completedLap);
                }
            }

            this.participantUpdatesSubject.OnNext(participant);
            this.entries[participant.Index] = participant;
        }
    }

    private void UpdateTelemetry(SharedMemoryData sharedMemoryData)
    {
        if(sharedMemoryData.FocusedParticipantIndex == -1)
        {
            return;
        }

        var telemetryFrame = sharedMemoryData.GetTelemetryFrame();
        this.LogMessage(LoggingLevel.Information, $"Telemetry Frame: {telemetryFrame}");
        this.telemetrySubject.OnNext(telemetryFrame);
    }
}