using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Pmr.Demo.Abstractions;

namespace SimRacingSdk.Pmr.Demo.Services;

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
