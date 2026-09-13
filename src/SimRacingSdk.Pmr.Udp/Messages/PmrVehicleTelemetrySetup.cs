namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetrySetup
{
    public float BrakeBias { get; init; }
    public float FrontAntiRollStiffness { get; init; }
    public float RearAntiRollStiffness { get; init; }
    public float RegenLimit { get; init; }
    public float DeployLimit { get; init; }
    public byte AbsLevel { get; init; }
    public byte TcsLevel { get; init; }
}
