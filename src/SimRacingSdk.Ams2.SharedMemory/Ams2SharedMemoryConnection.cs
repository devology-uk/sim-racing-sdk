﻿using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;

namespace SimRacingSdk.Ams2.SharedMemory;

public class Ams2SharedMemoryConnection : IAms2SharedMemoryConnection
{
    private readonly IAms2SharedMemoryProvider ams2SharedMemoryProvider;
    private readonly Subject<LogMessage> logMessagesSubject = new();

    private IDisposable? updateSubscription;

    public Ams2SharedMemoryConnection(IAms2SharedMemoryProvider ams2SharedMemoryProvider)
    {
        this.ams2SharedMemoryProvider = ams2SharedMemoryProvider;
    }

    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(double updateIntervalMs = 300)
    {
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

    private void LogMessage(LoggingLevel loggingLevel, string message, object? data = null)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel, message, data));
    }

    private void OnCompleted() { }

    private void OnError(Exception exception) { }

    private void OnNextUpdate(long index)
    {
        var sharedMemoryData = this.ams2SharedMemoryProvider.ReadSharedMemoryData();

        if(sharedMemoryData.SequenceNumber % 2 > 0)
        {
            return;
        }

        this.LogMessage(LoggingLevel.Information, sharedMemoryData.GetGameStatus().ToString());
        this.LogMessage(LoggingLevel.Information, sharedMemoryData.GetPlayer().ToString());
        this.LogMessage(LoggingLevel.Information, sharedMemoryData.GetTelemetryFrame().ToString());
    }
}