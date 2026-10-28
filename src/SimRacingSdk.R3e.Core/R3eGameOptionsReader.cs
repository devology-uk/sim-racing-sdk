using System.Text;
using SimRacingSdk.R3e.Core.Abstractions;

namespace SimRacingSdk.R3e.Core;

// game_options is an unencrypted typed key/value file: a header (magic, version), then entries of
// type tag, length-prefixed UTF-8 name and value until the end of the file. Integers and doubles
// are followed by trailing bytes of unknown meaning (4 and 8 respectively).
public class R3eGameOptionsReader : IR3eGameOptionsReader
{
    private const int DoubleTrailerLength = 8;
    private const int DoubleType = 1;
    private const int IntegerTrailerLength = 4;
    private const int IntegerType = 0;
    private const uint Magic = 0xFFFF7007;
    private const int StringType = 2;

    private static R3eGameOptionsReader? singletonInstance;

    public static R3eGameOptionsReader Instance => singletonInstance ??= new R3eGameOptionsReader();

    public IReadOnlyDictionary<string, object> Read(string filePath)
    {
        using var reader = new BinaryReader(File.OpenRead(filePath), Encoding.UTF8);
        this.ReadHeader(reader, filePath);

        var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        while(reader.BaseStream.Position < reader.BaseStream.Length)
        {
            var type = reader.ReadInt32();
            var name = this.ReadString(reader);
            values[name] = this.ReadValue(reader, type, name);
        }

        return values;
    }

    private void ReadHeader(BinaryReader reader, string filePath)
    {
        if(reader.ReadUInt32() != Magic)
        {
            throw new InvalidDataException($"{filePath} is not an R3E game options file.");
        }

        reader.ReadInt32();
    }

    private string ReadString(BinaryReader reader)
    {
        var length = reader.ReadInt32();
        return Encoding.UTF8.GetString(reader.ReadBytes(length));
    }

    private object ReadValue(BinaryReader reader, int type, string name)
    {
        return type switch
        {
            IntegerType => this.ReadInteger(reader),
            DoubleType => this.ReadDouble(reader),
            StringType => this.ReadString(reader),
            _ => throw new InvalidDataException($"Game option {name} has unknown value type {type}.")
        };
    }

    private int ReadInteger(BinaryReader reader)
    {
        var value = reader.ReadInt32();
        reader.ReadBytes(IntegerTrailerLength);
        return value;
    }

    private double ReadDouble(BinaryReader reader)
    {
        var value = reader.ReadDouble();
        reader.ReadBytes(DoubleTrailerLength);
        return value;
    }
}
