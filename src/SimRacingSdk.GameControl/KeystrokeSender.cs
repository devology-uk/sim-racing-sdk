using System.Runtime.InteropServices;
using SimRacingSdk.GameControl.Abstractions;
using SimRacingSdk.GameControl.Models;
using SimRacingSdk.GameControl.Native;

namespace SimRacingSdk.GameControl;

// Sends scan codes rather than virtual keys: games reading raw input or DirectInput ignore virtual-key-only events.
public class KeystrokeSender : IKeystrokeSender
{
    private static readonly HashSet<int> ExtendedVirtualKeys =
    [
        0x21, // Page Up
        0x22, // Page Down
        0x23, // End
        0x24, // Home
        0x25, // Left
        0x26, // Up
        0x27, // Right
        0x28, // Down
        0x2C, // Print Screen
        0x2D, // Insert
        0x2E, // Delete
        0x5B, // Left Windows
        0x5C, // Right Windows
        0x5D, // Applications
        0x6F, // Numpad Divide
        0x90, // Num Lock
        0xA3, // Right Control
        0xA5  // Right Alt
    ];

    private static readonly int InputSize = Marshal.SizeOf<Input>();

    public async Task<bool> PressAsync(GameKeyPress keyPress)
    {
        var keys = keyPress.ModifierVirtualKeys.Append(keyPress.VirtualKey)
                           .ToList();

        var keysDown = keys.Select(key => CreateKeyInput(key, isKeyUp: false))
                           .ToArray();
        var keysUp = Enumerable.Reverse(keys)
                               .Select(key => CreateKeyInput(key, isKeyUp: true))
                               .ToArray();

        var allDown = Send(keysDown);
        await Task.Delay(keyPress.HoldDuration);
        var allUp = Send(keysUp);
        return allDown && allUp;
    }

    private static Input CreateKeyInput(int virtualKey, bool isKeyUp)
    {
        return new Input
        {
            Type = NativeMethods.InputKeyboard,
            Union = new InputUnion
            {
                Keyboard = new KeyboardInput
                {
                    ScanCode = ToScanCode(virtualKey),
                    Flags = ToKeyFlags(virtualKey, isKeyUp)
                }
            }
        };
    }

    private static uint ToKeyFlags(int virtualKey, bool isKeyUp)
    {
        var flags = NativeMethods.KeyEventScanCode;
        if(ExtendedVirtualKeys.Contains(virtualKey))
        {
            flags |= NativeMethods.KeyEventExtendedKey;
        }

        if(isKeyUp)
        {
            flags |= NativeMethods.KeyEventKeyUp;
        }

        return flags;
    }

    private static ushort ToScanCode(int virtualKey)
    {
        return (ushort)NativeMethods.MapVirtualKey((uint)virtualKey, NativeMethods.MapVirtualKeyToScanCode);
    }

    private static bool Send(Input[] inputs)
    {
        var sentCount = NativeMethods.SendInput((uint)inputs.Length, inputs, InputSize);
        return sentCount == inputs.Length;
    }
}
