namespace SimRacingSdk.Acc.Demo.Abstractions;

public interface IConsoleLog
{
    IObservable<string> Entries { get; }
    void Write(string message);
}