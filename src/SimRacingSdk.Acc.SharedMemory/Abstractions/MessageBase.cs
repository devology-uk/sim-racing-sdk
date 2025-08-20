namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public abstract class MessageBase
{
    internal static string ReadString(BinaryReader binary, int length)
    {
        var result = string.Empty;

        if (length <= 0)
        {
            return result;
        }

        for(var i = 0; i < length; i++)
        {
            result += binary.ReadChar();
        }

        return result;
    }
}