#nullable disable

using System.IO.MemoryMappedFiles;
using SimRacingSdk.Acc.SharedMemory.Enums;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Messages;

public class GraphicsPage
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
            var reader = new BinaryReader(stream);

            graphicsPage = new GraphicsPage
            {
                PacketId = reader.ReadInt32(),
                Status = (AccRtStatus)reader.ReadInt32(),
                SessionType = (AccRtSessionType)reader.ReadInt32(),
                CurrentTime = reader.ReadString(),
                LastTime = reader.ReadString(),
                BestTime = reader.ReadString(),
                Split = reader.ReadString(),
                CompletedLaps = reader.ReadInt32(),
                Position = reader.ReadInt32(),
                CurrentTimeMs = reader.ReadInt32(),
                LastTimeMs = reader.ReadInt32(),
                BestTimeMs = reader.ReadInt32(),
                SessionTimeLeft = reader.ReadSingle(),
                DistanceTraveled = reader.ReadSingle(),
                IsInPits = reader.ReadBoolean(),
                CurrentSectorIndex = reader.ReadInt32(),
                LastSectorTime = reader.ReadInt32(),
                NumberOfLaps = reader.ReadInt32(),
                TyreCompound = reader.ReadString(),
                ReplayTimeMultiplier = reader.ReadSingle(),
                NormalizedCarPosition = reader.ReadSingle(),
                ActiveCars = reader.ReadInt32(),
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
                graphicsPage.CarIds[i] = reader.ReadInt32();
            }

            graphicsPage.PlayerCarID = reader.ReadInt32();
            graphicsPage.PenaltyTime = reader.ReadSingle();
            graphicsPage.Flag = (AccRtFlagType)reader.ReadInt32();
            graphicsPage.PenaltyType = (AccRtPenaltyShortcut)reader.ReadInt32();
            graphicsPage.IdealLineOn = reader.ReadBoolean();
            graphicsPage.IsInPitLane = reader.ReadBoolean();
            graphicsPage.SurfaceGrip = reader.ReadSingle();
            graphicsPage.MandatoryPitDone = reader.ReadBoolean();
            graphicsPage.WindSpeed = reader.ReadSingle();
            graphicsPage.WindDirection = reader.ReadSingle();
            graphicsPage.IsSetupMenuVisible = reader.ReadBoolean();
            graphicsPage.MainDisplayIndex = reader.ReadInt32();
            graphicsPage.SecondaryDisplayIndex = reader.ReadInt32();
            graphicsPage.TC = reader.ReadInt32();
            graphicsPage.TCCut = reader.ReadInt32();
            graphicsPage.EngineMap = reader.ReadInt32();
            graphicsPage.ABS = reader.ReadInt32();
            graphicsPage.AverageFuelPerLap = reader.ReadSingle();
            graphicsPage.RainLights = reader.ReadInt32();
            graphicsPage.FlashingLights = reader.ReadInt32();
            graphicsPage.LightsStage = reader.ReadInt32();
            graphicsPage.ExhaustTemperature = reader.ReadSingle();
            graphicsPage.WiperLV = reader.ReadInt32();
            graphicsPage.DriverStintTotalTimeLeft = reader.ReadInt32();
            graphicsPage.DriverStintTimeLeft = reader.ReadInt32();
            graphicsPage.RainTyres = reader.ReadInt32();
            graphicsPage.SessionIndex = reader.ReadInt32();
            graphicsPage.UsedFuelSinceRefuel = reader.ReadSingle();
            graphicsPage.DeltaLapTime = reader.ReadString();
            graphicsPage.DeltaLapTimeMillis = reader.ReadInt32();
            graphicsPage.EstimatedLapTime = reader.ReadString();
            graphicsPage.EstimatedLapTimeMillis = reader.ReadInt32();
            graphicsPage.IsDeltaPositive = reader.ReadBoolean();
            graphicsPage.SplitTimeMillis = reader.ReadInt32();
            graphicsPage.IsValidLap = reader.ReadBoolean();
            graphicsPage.FuelEstimatedLaps = reader.ReadSingle();
            graphicsPage.TrackStatus = reader.ReadString();
            graphicsPage.MandatoryPitStopsLeft = reader.ReadInt32();
            graphicsPage.ClockTimeOfDaySeconds = reader.ReadSingle();
            graphicsPage.IndicatorLeftOn = reader.ReadBoolean();
            graphicsPage.IndicatorRightOn = reader.ReadBoolean();
            graphicsPage.GlobalYellow = reader.ReadBoolean();
            graphicsPage.GlobalYellowSector1 = reader.ReadBoolean();
            graphicsPage.GlobalYellowSector2 = reader.ReadBoolean();
            graphicsPage.GlobalYellowSector3 = reader.ReadBoolean();
            graphicsPage.GlobalWhite = reader.ReadBoolean();
            graphicsPage.GreenFlag = reader.ReadBoolean();
            graphicsPage.GlobalChequered = reader.ReadBoolean();
            graphicsPage.GlobalRed = reader.ReadBoolean();
            graphicsPage.MfdTyreSet = reader.ReadInt32();
            graphicsPage.MfdFuelToAdd = reader.ReadSingle();
            graphicsPage.MfdTyrePressureLF = reader.ReadSingle();
            graphicsPage.MfdTyrePressureRF = reader.ReadSingle();
            graphicsPage.MfdTyrePressureLR = reader.ReadSingle();
            graphicsPage.MfdTyrePressureRR = reader.ReadSingle();
            graphicsPage.TrackGripStatus = (AccRtTrackGripStatus)reader.ReadInt32();
            graphicsPage.RainIntensity = (AccRtRainIntensity)reader.ReadInt32();
            graphicsPage.RainIntensityIn10min = (AccRtRainIntensity)reader.ReadInt32();
            graphicsPage.RainIntensityIn30min = (AccRtRainIntensity)reader.ReadInt32();
            graphicsPage.CurrentTyreSet = reader.ReadInt32();
            graphicsPage.StrategyTyreSet = reader.ReadInt32();
            graphicsPage.GapAheadMillis = reader.ReadInt32();
            graphicsPage.GapBehindMillis = reader.ReadInt32();

            return true;

        }
        catch(FileNotFoundException)
        {
            graphicsPage = null;
            return false;
        }
    }
}