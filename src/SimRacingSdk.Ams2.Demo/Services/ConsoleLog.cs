﻿using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Ams2.Demo.Abstractions;

namespace SimRacingSdk.Ams2.Demo.Services;

public class ConsoleLog : IConsoleLog
{
    private readonly Subject<string> entriesSubject = new();

    public event Action Cleared = null!;

    public IObservable<string> Entries => this.entriesSubject.AsObservable();

    public void Write(string message)
    {
        this.entriesSubject.OnNext(message);
    }

    public void Clear()
    {
        this.Cleared?.Invoke();
    }
}