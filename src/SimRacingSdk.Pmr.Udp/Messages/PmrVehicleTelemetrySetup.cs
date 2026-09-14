namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetrySetup
{
    public float BrakeBiasFraction { get; init; }

    // Not empirically confirmed - no captured session has shown a non-default value to
    // sanity-check magnitude/unit against.
    public float FrontAntiRollStiffnessNmPerRadian { get; init; }

    public float RearAntiRollStiffnessNmPerRadian { get; init; }
    public float RegenLimitFraction { get; init; }
    public float DeployLimitFraction { get; init; }
    public byte AbsLevel { get; init; }
    public byte TcsLevel { get; init; }
}
