using SimRacingSdk.Acc.Core.Messages;

namespace SimRacingSdk.Ams2.Udp.Abstractions;

public interface IAms2UdpConnection: IDisposable
{
    string ConnectionIdentifier { get; }
    string IpAddress { get; }
    int Port { get; }
    IObservable<LogMessage> LogMessages { get; }
    void Connect(bool autoDetect = true);
    void Dispose();
    void Stop();
}