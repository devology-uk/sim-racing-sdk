#nullable disable

using System.IO.MemoryMappedFiles;
using System.Text;
using SimRacingSdk.Acc.SharedMemory.Abstractions;

namespace SimRacingSdk.Acc.SharedMemory.Messages;

public class StaticDataPage : MessageBase
{
    private const string StaticMap = "Local\\acpmf_static";

    public string AccVersion = string.Empty;
    public bool AidAllowTyreBlankets;
    public bool AidAutoBlip;
    public bool AidAutoClutch;
    public float AidFuelRate;
    public float AidMechanicalDamage;
    public float AidStability;
    public float AidTireRate;
    public string CarModel = string.Empty;
    public string DryTyresName = string.Empty;
    public bool IsOnline;
    public float MaxFuel;
    public int MaxRpm;
    public int NumberOfCars;
    public int NumberOfSessions;
    public bool PenaltiesEnabled;
    public int PitWindowEnd;
    public int PitWindowStart;
    public string PlayerFirstName = string.Empty;
    public string PlayerNickname = string.Empty;
    public string PlayerSurname = string.Empty;
    public int SectorCount;
    public string SharedMemoryVersion = string.Empty;
    public string TrackName = string.Empty;
    public string WetTyresName = string.Empty;

    public static bool TryRead(out StaticDataPage staticDataPage)
    {
        try
        {
            using var mappedFile = MemoryMappedFile.OpenExisting(StaticMap, MemoryMappedFileRights.Read);
            using var stream = mappedFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
            var reader = new BinaryReader(stream, Encoding.Unicode);
            staticDataPage = new StaticDataPage
            {
                SharedMemoryVersion = ReadString(reader, 15),
                AccVersion = ReadString(reader, 15),
                NumberOfSessions = (int)reader.ReadUInt32(),
                NumberOfCars = (int)reader.ReadUInt32(),
                CarModel = ReadString(reader, 33),
                TrackName = ReadString(reader, 33),
                PlayerFirstName = ReadString(reader, 33),
                PlayerSurname = ReadString(reader, 33),
                PlayerNickname = ReadString(reader, 33),
                SectorCount = (int)reader.ReadUInt32(),
                MaxTorque = reader.ReadSingle(),
                MaxPower = reader.ReadSingle(),
                MaxRpm = (int)reader.ReadUInt32(),
                MaxFuel = reader.ReadSingle(),
                SuspensionMaxTravel =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                ],
                TyreRadius =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                ],
                MaxTurboBoost = reader.ReadSingle(),
                AirTemperature = reader.ReadSingle(),
                RoadTemperature = reader.ReadSingle(),
                PenaltiesEnabled = reader.ReadUInt32() > 0,
                AidFuelRate = reader.ReadSingle(),
                AidTireRate = reader.ReadSingle(),
                AidMechanicalDamage = reader.ReadSingle(),
                AidAllowTyreBlankets = reader.ReadUInt32() > 0,
                AidStability = reader.ReadSingle(),
                AidAutoClutch = reader.ReadUInt32() > 0,
                AidAutoBlip = reader.ReadUInt32() > 0,
                HasDRS = reader.ReadUInt32() > 0,
                HasERS = reader.ReadUInt32() > 0,
                HasKERS = reader.ReadUInt32() > 0,
                KersMaxJoules = reader.ReadSingle(),
                EngineBrakeSettingsCount = (int)reader.ReadUInt32(),
                ErsPowerControllerCount = (int)reader.ReadUInt32(),
                TrackSplineLength = reader.ReadSingle(),
                TrackConfiguration = ReadString(reader, 33),
                ErsMaxJ = reader.ReadSingle(),
                IsTimedRace = reader.ReadUInt32() > 0,
                HasExtraLap = reader.ReadUInt32() > 0,
                CarSkin = ReadString(reader, 33),
                ReversedGridPositions = (int)reader.ReadUInt32(),
                PitWindowStart = (int)reader.ReadUInt32(),
                PitWindowEnd = (int)reader.ReadUInt32(),
                IsOnline = reader.ReadUInt32() > 0,
                DryTyresName = ReadString(reader, 33),
                WetTyresName = ReadString(reader, 33)
            };

            return true;
        }
        catch(FileNotFoundException)
        {
            staticDataPage = null;
            return false;
        }
    }

    # region Not populated by ACC

    public float MaxTorque;
    public float MaxPower;
    public float[] SuspensionMaxTravel;
    public float[] TyreRadius;
    public float MaxTurboBoost;
    public float AirTemperature;
    public float RoadTemperature;
    public bool HasDRS;
    public bool HasERS;
    public bool HasKERS;
    public float KersMaxJoules;
    public int EngineBrakeSettingsCount;
    public int ErsPowerControllerCount;
    public float TrackSplineLength;
    public string TrackConfiguration = string.Empty;
    public float ErsMaxJ;
    public bool IsTimedRace;
    public bool HasExtraLap;
    public string CarSkin = string.Empty;
    public int ReversedGridPositions;

    #endregion
}