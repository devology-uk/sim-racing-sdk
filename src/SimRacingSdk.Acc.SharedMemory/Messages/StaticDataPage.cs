#nullable disable

using System.IO.MemoryMappedFiles;

namespace SimRacingSdk.Acc.SharedMemory.Messages;

public class StaticDataPage
{
    private const string StaticMap = "Local\\acpmf_static";

    public string AccVersion;
    public bool AidAllowTyreBlankets;
    public bool AidAutoBlip;
    public bool AidAutoClutch;
    public float AidFuelRate;
    public float AidMechanicalDamage;
    public float AidStability;
    public float AidTireRate;
    public string CarModel;
    public string DryTyresName;
    public bool IsOnline;
    public float MaxFuel;
    public int MaxRpm;
    public int NumberOfCars;
    public int NumberOfSessions;
    public bool PenaltiesEnabled;
    public int PitWindowEnd;
    public int PitWindowStart;
    public string PlayerFirstName;
    public string PlayerNickname;
    public string PlayerSurname;
    public int SectorCount;
    public string SharedMemoryVersion;
    public string TrackName;
    public string WetTyresName;

    public static bool TryRead(out StaticDataPage staticDataPage)
    {
        try
        {
            using var mappedFile = MemoryMappedFile.OpenExisting(StaticMap, MemoryMappedFileRights.Read);
            using var stream = mappedFile.CreateViewStream();
            var reader = new BinaryReader(stream);
            staticDataPage = new StaticDataPage
            {
                SharedMemoryVersion = reader.ReadString(),
                AccVersion = reader.ReadString(),
                NumberOfSessions = reader.ReadInt32(),
                NumberOfCars = reader.ReadInt32(),
                CarModel = reader.ReadString(),
                TrackName = reader.ReadString(),
                PlayerFirstName = reader.ReadString(),
                PlayerSurname = reader.ReadString(),
                PlayerNickname = reader.ReadString(),
                SectorCount = reader.ReadInt32(),
                MaxTorque = reader.ReadSingle(),
                MaxPower = reader.ReadSingle(),
                MaxRpm = reader.ReadInt32(),
                MaxFuel = reader.ReadSingle(),
                SuspensionMaxTravel =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TyreRadius =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                MaxTurboBoost = reader.ReadSingle(),
                AirTemperature = reader.ReadSingle(),
                RoadTemperature = reader.ReadSingle(),
                PenaltiesEnabled = reader.ReadBoolean(),
                AidFuelRate = reader.ReadSingle(),
                AidTireRate = reader.ReadSingle(),
                AidMechanicalDamage = reader.ReadSingle(),
                AidAllowTyreBlankets = reader.ReadBoolean(),
                AidStability = reader.ReadSingle(),
                AidAutoClutch = reader.ReadBoolean(),
                AidAutoBlip = reader.ReadBoolean(),
                HasDRS = reader.ReadBoolean(),
                HasERS = reader.ReadBoolean(),
                HasKERS = reader.ReadBoolean(),
                KersMaxJoules = reader.ReadSingle(),
                EngineBrakeSettingsCount = reader.ReadInt32(),
                ErsPowerControllerCount = reader.ReadInt32(),
                TrackSplineLength = reader.ReadSingle(),
                TrackConfiguration = reader.ReadString(),
                ErsMaxJ = reader.ReadSingle(),
                IsTimedRace = reader.ReadBoolean(),
                HasExtraLap = reader.ReadBoolean(),
                CarSkin = reader.ReadString(),
                ReversedGridPositions = reader.ReadInt32(),
                PitWindowStart = reader.ReadInt32(),
                PitWindowEnd = reader.ReadInt32(),
                IsOnline = reader.ReadBoolean(),
                DryTyresName = reader.ReadString(),
                WetTyresName = reader.ReadString()
            };

            return true;

        }
        catch (FileNotFoundException)
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
    public string TrackConfiguration;
    public float ErsMaxJ;
    public bool IsTimedRace;
    public bool HasExtraLap;
    public string CarSkin;
    public int ReversedGridPositions;

    #endregion
}