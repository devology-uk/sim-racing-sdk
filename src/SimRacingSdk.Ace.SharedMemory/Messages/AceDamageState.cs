#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// SMEvoDamageState from the PDF, fixed at 128 bytes. See AceTyreState.cs for why a trailing
// Reserved array is used to hit that documented size - unverified against a live game.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct AceDamageState
{
    public float DamageFront;
    public float DamageRear;
    public float DamageLeft;
    public float DamageRight;
    public float DamageCenter;
    public float DamageSuspensionLf;
    public float DamageSuspensionRf;
    public float DamageSuspensionLr;
    public float DamageSuspensionRr;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 92)]
    public byte[] Reserved;
}
