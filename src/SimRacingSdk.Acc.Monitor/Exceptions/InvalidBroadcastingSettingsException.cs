namespace SimRacingSdk.Acc.Monitor.Exceptions;

public class InvalidBroadcastingSettingsException : Exception
{
    public InvalidBroadcastingSettingsException()
        : base("The local ACC installation has not been configured for broadcasting.") { }
}