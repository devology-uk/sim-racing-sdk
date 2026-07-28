namespace SimRacingSdk.Ace.Monitor.Exceptions;

public class InvalidBroadcastingSettingsException : Exception
{
    public InvalidBroadcastingSettingsException()
        : base("The local Ace Evo installation has not been configured for broadcasting.") { }
}
