using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Demo.Abstractions;

namespace SimRacingSdk.Acc.Demo.Services;

public class ConsoleLog : IConsoleLog
{
    private readonly Subject<string> entriesSubject = new();
    public IObservable<string> Entries => this.entriesSubject.AsObservable();

    public void Write(string message)
    {
        this.entriesSubject.OnNext(message);
    }
}