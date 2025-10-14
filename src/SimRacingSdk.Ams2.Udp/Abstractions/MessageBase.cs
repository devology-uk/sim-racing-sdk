using SimRacingSdk.Ams2.Udp.Messages;

namespace SimRacingSdk.Ams2.Udp.Abstractions;

public abstract record MessageBase
{
    protected MessageBase(MessageHeader header)
    {
        this.Header = header;
    }

    public MessageHeader Header { get; }
}