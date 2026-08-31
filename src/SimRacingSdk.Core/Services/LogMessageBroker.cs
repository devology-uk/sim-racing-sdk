using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Core.Services;

public class LogMessageBroker
{
    private readonly string defaultSource;
    private readonly Subject<LogMessage> logMessagesSubject = new();

    public LogMessageBroker(string defaultSource)
    {
        this.defaultSource = defaultSource;
    }

    public IObservable<LogMessage> Messages => this.logMessagesSubject.AsObservable();

    public void Log(LoggingLevel level, string content, string? source = null)
    {
        this.logMessagesSubject.OnNext(new LogMessage(level, content, source ?? this.defaultSource));
    }

    public void Relay(LogMessage message)
    {
        this.logMessagesSubject.OnNext(message);
    }
}
