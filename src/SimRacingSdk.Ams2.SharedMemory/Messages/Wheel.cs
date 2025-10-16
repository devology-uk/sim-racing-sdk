using System.Runtime.InteropServices;

namespace SimRacingSdk.Ams2.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential)]
public struct Wheel<T>
{
    public T FrontLeft;
    public T FrontRight;
    public T RearLeft;
    public T RearRight;
}
