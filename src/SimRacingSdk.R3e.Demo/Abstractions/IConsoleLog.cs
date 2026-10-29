namespace SimRacingSdk.R3e.Demo.Abstractions;

public interface IConsoleLog
{
    event Action Cleared;
    IObservable<string> Entries { get; }
    void Clear();
    void Write(string message);
}
