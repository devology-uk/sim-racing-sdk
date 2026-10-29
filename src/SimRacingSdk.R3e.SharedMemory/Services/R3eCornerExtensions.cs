using SimRacingSdk.R3e.SharedMemory.Enums;
using SimRacingSdk.R3e.SharedMemory.Messages;

namespace SimRacingSdk.R3e.SharedMemory.Services;

internal static class R3eCornerExtensions
{
    public static float For(this R3eWheelsSingle wheels, R3eCorner corner)
    {
        return corner switch
        {
            R3eCorner.FrontLeft => wheels.FrontLeft,
            R3eCorner.FrontRight => wheels.FrontRight,
            R3eCorner.RearLeft => wheels.RearLeft,
            _ => wheels.RearRight
        };
    }

    public static double For(this R3eWheelsDouble wheels, R3eCorner corner)
    {
        return corner switch
        {
            R3eCorner.FrontLeft => wheels.FrontLeft,
            R3eCorner.FrontRight => wheels.FrontRight,
            R3eCorner.RearLeft => wheels.RearLeft,
            _ => wheels.RearRight
        };
    }

    public static int For(this R3eWheelsInt wheels, R3eCorner corner)
    {
        return corner switch
        {
            R3eCorner.FrontLeft => wheels.FrontLeft,
            R3eCorner.FrontRight => wheels.FrontRight,
            R3eCorner.RearLeft => wheels.RearLeft,
            _ => wheels.RearRight
        };
    }

    public static R3eTyreTemperatureInformation For(this R3eWheelsTyreTemperature wheels, R3eCorner corner)
    {
        return corner switch
        {
            R3eCorner.FrontLeft => wheels.FrontLeft,
            R3eCorner.FrontRight => wheels.FrontRight,
            R3eCorner.RearLeft => wheels.RearLeft,
            _ => wheels.RearRight
        };
    }

    public static R3eBrakeTemperature For(this R3eWheelsBrakeTemperature wheels, R3eCorner corner)
    {
        return corner switch
        {
            R3eCorner.FrontLeft => wheels.FrontLeft,
            R3eCorner.FrontRight => wheels.FrontRight,
            R3eCorner.RearLeft => wheels.RearLeft,
            _ => wheels.RearRight
        };
    }
}
