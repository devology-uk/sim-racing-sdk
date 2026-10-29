#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

// High-precision physics for the player's car only.
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3ePlayerData
{
    public int UserId;
    public int GameSimulationTicks;
    public double GameSimulationTime;
    public R3eVector3Double Position;
    public R3eVector3Double Velocity;
    public R3eVector3Double LocalVelocity;
    public R3eVector3Double Acceleration;
    public R3eVector3Double LocalAcceleration;
    public R3eVector3Double Orientation;
    public R3eVector3Double Rotation;
    public R3eVector3Double AngularAcceleration;
    public R3eVector3Double AngularVelocity;
    public R3eVector3Double LocalAngularVelocity;
    public R3eVector3Double LocalGforce;
    public double SteeringForce;
    public double SteeringForcePercentage;
    public double EngineTorque;
    public double CurrentDownforce;
    public double Voltage;
    public double ErsLevel;
    public double PowerMguH;
    public double PowerMguK;
    public double TorqueMguK;
    public R3eWheelsDouble SuspensionDeflection;
    public R3eWheelsDouble SuspensionVelocity;
    public R3eWheelsDouble Camber;
    public R3eWheelsDouble RideHeight;
    public double FrontWingHeight;
    public double FrontRollAngle;
    public double RearRollAngle;
    public double ThirdSpringSuspensionDeflectionFront;
    public double ThirdSpringSuspensionVelocityFront;
    public double ThirdSpringSuspensionDeflectionRear;
    public double ThirdSpringSuspensionVelocityRear;
    public double Unused1;
    public double Unused2;
    public double Unused3;
}
