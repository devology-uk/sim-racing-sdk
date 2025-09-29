using SimRacingSdk.Ams2.Udp.Messages;

namespace SimRacingSdk.Ams2.Udp.Abstractions;

public abstract record MessageBase
{
    protected MessageBase(MessageHeader messageHeader)
    {
        this.Header = messageHeader;
    }

    public MessageHeader Header { get; }
}