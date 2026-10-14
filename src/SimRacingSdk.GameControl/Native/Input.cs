using System.Runtime.InteropServices;

namespace SimRacingSdk.GameControl.Native;

// Mirrors the Win32 INPUT struct. The union carries the mouse member, though it is never used,
// because it is the largest member and SendInput rejects an INPUT of the wrong size.
[StructLayout(LayoutKind.Sequential)]
internal struct Input
{
    public uint Type;
    public InputUnion Union;
}

[StructLayout(LayoutKind.Explicit)]
internal struct InputUnion
{
    [FieldOffset(0)]
    public MouseInput Mouse;

    [FieldOffset(0)]
    public KeyboardInput Keyboard;
}

[StructLayout(LayoutKind.Sequential)]
internal struct MouseInput
{
    public int X;
    public int Y;
    public uint MouseData;
    public uint Flags;
    public uint Time;
    public IntPtr ExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
internal struct KeyboardInput
{
    public ushort VirtualKey;
    public ushort ScanCode;
    public uint Flags;
    public uint Time;
    public IntPtr ExtraInfo;
}
