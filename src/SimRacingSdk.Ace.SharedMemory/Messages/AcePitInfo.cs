#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// SMEvoPitInfo from the PDF, fixed at 64 bytes. See AceTyreState.cs for why a trailing
// Reserved array is used to hit that documented size - unverified against a live game.
// -1 = will not perform, 0 = completed, 1 = in progress, per field.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct AcePitInfo
{
    public sbyte Damage;
    public sbyte Fuel;
    public sbyte TyresLf;
    public sbyte TyresRf;
    public sbyte TyresLr;
    public sbyte TyresRr;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 58)]
    public byte[] Reserved;

    public override readonly string ToString()
    {
        return $"AcePitInfo {{ Damage = {this.Damage}, Fuel = {this.Fuel}, TyresLf = {this.TyresLf}, "
             + $"TyresRf = {this.TyresRf}, TyresLr = {this.TyresLr}, TyresRr = {this.TyresRr} }}";
    }
}
