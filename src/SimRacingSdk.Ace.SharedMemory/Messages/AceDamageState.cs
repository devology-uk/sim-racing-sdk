#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

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

    public override readonly string ToString()
    {
        return $"AceDamageState {{ DamageFront = {this.DamageFront}, DamageRear = {this.DamageRear}, "
             + $"DamageLeft = {this.DamageLeft}, DamageRight = {this.DamageRight}, DamageCenter = {this.DamageCenter}, "
             + $"DamageSuspensionLf = {this.DamageSuspensionLf}, DamageSuspensionRf = {this.DamageSuspensionRf}, "
             + $"DamageSuspensionLr = {this.DamageSuspensionLr}, DamageSuspensionRr = {this.DamageSuspensionRr} }}";
    }
}
