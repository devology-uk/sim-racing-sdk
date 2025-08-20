#nullable disable

using System.IO.MemoryMappedFiles;
using System.Text;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Enums;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Messages;

public class GraphicsPage: MessageBase
{
    private const string GraphicsMap = "Local\\acpmf_graphics";

    public int ABS;
    public int ActiveCars;
    public float AverageFuelPerLap;
    public string BestTime;
    public int BestTimeMs;
    public AccRtVector3d[] CarCoordinates;
    public int[] CarIds;
    public float ClockTimeOfDaySeconds;
    public int CompletedLaps;
    public int CurrentSectorIndex;
    public string CurrentTime;
    public int CurrentTimeMs;
    public int CurrentTyreSet;
    public string DeltaLapTime;
    public int DeltaLapTimeMillis;
    public float DistanceTraveled;
    public int DriverStintTimeLeft;
    public int DriverStintTotalTimeLeft;
    public int EngineMap;
    public string EstimatedLapTime;
    public int EstimatedLapTimeMillis;
    public float ExhaustTemperature;
    public AccRtFlagType Flag;
    public int FlashingLights;
    public float FuelEstimatedLaps;
    public int GapAheadMillis;
    public int GapBehindMillis;
    public bool GlobalChequered;
    public bool GlobalRed;
    public bool GlobalWhite;
    public bool GlobalYellow;
    public bool GlobalYellowSector1;
    public bool GlobalYellowSector2;
    public bool GlobalYellowSector3;
    public bool GreenFlag;
    public bool IdealLineOn;
    public bool IndicatorLeftOn;
    public bool IndicatorRightOn;
    public bool IsDeltaPositive;
    public bool IsInPitLane;
    public bool IsInPits;
    public bool IsSetupMenuVisible;
    public bool IsValidLap;
    public int LastSectorTime;
    public string LastTime;
    public int LastTimeMs;
    public int LightsStage;
    public int MainDisplayIndex;
    public bool MandatoryPitDone;
    public int MandatoryPitStopsLeft;
    public float MfdFuelToAdd;
    public float MfdTyrePressureLF;
    public float MfdTyrePressureLR;
    public float MfdTyrePressureRF;
    public float MfdTyrePressureRR;
    public int MfdTyreSet;
    public int NumberOfLaps;
    public int PacketId;
    public float PenaltyTime;
    public AccRtPenaltyShortcut PenaltyType;
    public int PlayerCarID;
    public int Position;
    public AccRtRainIntensity RainIntensity;
    public AccRtRainIntensity RainIntensityIn10min;
    public AccRtRainIntensity RainIntensityIn30min;
    public int RainLights;
    public int RainTyres;
    public float ReplayTimeMultiplier;
    public int SecondaryDisplayIndex;
    public int SessionIndex;
    public float SessionTimeLeft;
    public AccRtSessionType SessionType;
    public string Split;
    public int SplitTimeMillis;
    public AccRtStatus Status;
    public int StrategyTyreSet;
    public float SurfaceGrip;
    public int TC;
    public int TCCut;
    public AccRtTrackGripStatus TrackGripStatus;
    public string TrackStatus;
    public string TyreCompound;
    public float UsedFuelSinceRefuel;
    public float WindDirection;
    public float WindSpeed;
    public int WiperLV;
    
    #region Not populated by ACC

    public float NormalizedCarPosition;

    #endregion

    public static bool TryRead(out GraphicsPage graphicsPage)
    {
        try
        {
            using var mappedFile = MemoryMappedFile.OpenExisting(GraphicsMap, MemoryMappedFileRights.Read);
            using var stream = mappedFile.CreateViewStream(0,0, MemoryMappedFileAccess.Read);
            var reader = new BinaryReader(stream, Encoding.Unicode);

            graphicsPage = new GraphicsPage
            {
                PacketId = (int)reader.ReadUInt32(),
                Status = (AccRtStatus)(int)reader.ReadUInt32(),
                SessionType = (AccRtSessionType)(int)reader.ReadUInt32(),
                CurrentTime = ReadString(reader, 15),
                LastTime = ReadString(reader, 15),
                BestTime = ReadString(reader, 15),
                Split = ReadString(reader, 15),
                CompletedLaps = (int)reader.ReadUInt32(),
                Position = (int)reader.ReadUInt32(),
                CurrentTimeMs = (int)reader.ReadUInt32(),
                LastTimeMs = (int)reader.ReadUInt32(),
                BestTimeMs = (int)reader.ReadUInt32(),
                SessionTimeLeft = reader.ReadSingle(),
                DistanceTraveled = reader.ReadSingle(),
                IsInPits = reader.ReadUInt32() > 0,
                CurrentSectorIndex = (int)reader.ReadUInt32(),
                LastSectorTime = (int)reader.ReadUInt32(),
                NumberOfLaps = (int)reader.ReadUInt32(),
                TyreCompound = ReadString(reader, 33),
                ReplayTimeMultiplier = reader.ReadSingle(),
                NormalizedCarPosition = reader.ReadSingle(),
                ActiveCars = (int)reader.ReadUInt32(),
                CarCoordinates = new AccRtVector3d[60]
            };

            for (var i = 0; i < graphicsPage.CarCoordinates.Length; i++)
            {
                graphicsPage.CarCoordinates[i] = new AccRtVector3d
                {
                    X = reader.ReadSingle(),
                    Y = reader.ReadSingle(),
                    Z = reader.ReadSingle()
                };
            }

            graphicsPage.CarIds = new int[60];
            for (var i = 0; i < graphicsPage.CarIds.Length; i++)
            {
                graphicsPage.CarIds[i] = (int)reader.ReadUInt32();
            }

            graphicsPage.PlayerCarID = (int)reader.ReadUInt32();
            graphicsPage.PenaltyTime = reader.ReadSingle();
            graphicsPage.Flag = (AccRtFlagType)(int)reader.ReadUInt32();
            graphicsPage.PenaltyType = (AccRtPenaltyShortcut)(int)reader.ReadUInt32();
            graphicsPage.IdealLineOn = reader.ReadUInt32() > 0;
            graphicsPage.IsInPitLane = reader.ReadUInt32() > 0;
            graphicsPage.SurfaceGrip = reader.ReadSingle();
            graphicsPage.MandatoryPitDone = reader.ReadUInt32() > 0;
            graphicsPage.WindSpeed = reader.ReadSingle();
            graphicsPage.WindDirection = reader.ReadSingle();
            graphicsPage.IsSetupMenuVisible = reader.ReadUInt32() > 0;
            graphicsPage.MainDisplayIndex = (int)reader.ReadUInt32();
            graphicsPage.SecondaryDisplayIndex = (int)reader.ReadUInt32();
            graphicsPage.TC = (int)reader.ReadUInt32();
            graphicsPage.TCCut = (int)reader.ReadUInt32();
            graphicsPage.EngineMap = (int)reader.ReadUInt32();
            graphicsPage.ABS = (int)reader.ReadUInt32();
            graphicsPage.AverageFuelPerLap = reader.ReadSingle();
            graphicsPage.RainLights = (int)reader.ReadUInt32();
            graphicsPage.FlashingLights = (int)reader.ReadUInt32();
            graphicsPage.LightsStage = (int)reader.ReadUInt32();
            graphicsPage.ExhaustTemperature = reader.ReadSingle();
            graphicsPage.WiperLV = (int)reader.ReadUInt32();
            graphicsPage.DriverStintTotalTimeLeft = (int)reader.ReadUInt32();
            graphicsPage.DriverStintTimeLeft = (int)reader.ReadUInt32();
            graphicsPage.RainTyres = (int)reader.ReadUInt32();
            graphicsPage.SessionIndex = (int)reader.ReadUInt32();
            graphicsPage.UsedFuelSinceRefuel = reader.ReadSingle();
            graphicsPage.DeltaLapTime = ReadString(reader, 15);
            graphicsPage.DeltaLapTimeMillis = (int)reader.ReadUInt32();
            graphicsPage.EstimatedLapTime = ReadString(reader, 15);
            graphicsPage.EstimatedLapTimeMillis = (int)reader.ReadUInt32();
            graphicsPage.IsDeltaPositive = reader.ReadUInt32() > 0;
            graphicsPage.SplitTimeMillis = (int)reader.ReadUInt32();
            graphicsPage.IsValidLap = reader.ReadUInt32() > 0;
            graphicsPage.FuelEstimatedLaps = reader.ReadSingle();
            graphicsPage.TrackStatus = ReadString(reader, 33);
            graphicsPage.MandatoryPitStopsLeft = (int)reader.ReadUInt32();
            graphicsPage.ClockTimeOfDaySeconds = reader.ReadSingle();
            graphicsPage.IndicatorLeftOn = reader.ReadUInt32() > 0;
            graphicsPage.IndicatorRightOn = reader.ReadUInt32() > 0;
            graphicsPage.GlobalYellow = reader.ReadUInt32() > 0;
            graphicsPage.GlobalYellowSector1 = reader.ReadUInt32() > 0;
            graphicsPage.GlobalYellowSector2 = reader.ReadUInt32() > 0;
            graphicsPage.GlobalYellowSector3 = reader.ReadUInt32() > 0;
            graphicsPage.GlobalWhite = reader.ReadUInt32() > 0;
            graphicsPage.GreenFlag = reader.ReadUInt32() > 0;
            graphicsPage.GlobalChequered = reader.ReadUInt32() > 0;
            graphicsPage.GlobalRed = reader.ReadUInt32() > 0;
            graphicsPage.MfdTyreSet = (int)reader.ReadUInt32();
            graphicsPage.MfdFuelToAdd = reader.ReadSingle();
            graphicsPage.MfdTyrePressureLF = reader.ReadSingle();
            graphicsPage.MfdTyrePressureRF = reader.ReadSingle();
            graphicsPage.MfdTyrePressureLR = reader.ReadSingle();
            graphicsPage.MfdTyrePressureRR = reader.ReadSingle();
            graphicsPage.TrackGripStatus = (AccRtTrackGripStatus)(int)reader.ReadUInt32();
            graphicsPage.RainIntensity = (AccRtRainIntensity)(int)reader.ReadUInt32();
            graphicsPage.RainIntensityIn10min = (AccRtRainIntensity)(int)reader.ReadUInt32();
            graphicsPage.RainIntensityIn30min = (AccRtRainIntensity)(int)reader.ReadUInt32();
            graphicsPage.CurrentTyreSet = (int)reader.ReadUInt32();
            graphicsPage.StrategyTyreSet = (int)reader.ReadUInt32();
            graphicsPage.GapAheadMillis = (int)reader.ReadUInt32();
            graphicsPage.GapBehindMillis = (int)reader.ReadUInt32();

            return true;

        }
        catch(FileNotFoundException)
        {
            graphicsPage = null;
            return false;
        }
    }
}