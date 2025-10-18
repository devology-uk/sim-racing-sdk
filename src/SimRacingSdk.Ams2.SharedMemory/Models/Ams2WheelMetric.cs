namespace SimRacingSdk.Ams2.SharedMemory.Models;

public record Ams2WheelMetric<T>(T FrontLeft, T FrontRight, T RearLeft, T RearRight) { }