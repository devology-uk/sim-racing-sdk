using System.Runtime.InteropServices;

namespace SimRacingSdk.GameControl.Native;

internal static class NativeMethods
{
    public const uint InputKeyboard = 1;
    public const uint KeyEventExtendedKey = 0x0001;
    public const uint KeyEventKeyUp = 0x0002;
    public const uint KeyEventScanCode = 0x0008;
    public const uint MapVirtualKeyToScanCode = 0;
    public const int ShowWindowRestore = 9;

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsIconic(IntPtr windowHandle);

    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint code, uint mapType);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint SendInput(uint inputCount, Input[] inputs, int inputSize);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr windowHandle);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr windowHandle, int command);
}
