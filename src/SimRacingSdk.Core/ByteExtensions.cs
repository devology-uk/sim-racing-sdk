namespace SimRacingSdk.Core;

public static class ByteExtensions
{
    public static int BottomFourBits(this byte val)
    {
        return val & 15;
    }

    public static int BottomSevenBits(this byte val)
    {
        return val & 127;
    }

    public static int BottomThreeBits(this byte val)
    {
        return val & 7;
    }

    public static int FourthBit(this byte val)
    {
        return val & 8;
    }

    public static int SecondTwoBits(this byte val)
    {
        return (val & 48) >> 4;
    }

    public static int TopBit(this byte val)
    {
        return val & 128;
    }

    public static int TopFourBits(this byte val)
    {
        return (val & 240) >> 4;
    }

    public static int TopTwoBits(this byte val)
    {
        return (val & 192) >> 6;
    }
}