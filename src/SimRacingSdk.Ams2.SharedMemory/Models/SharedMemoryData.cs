#nullable disable

using System.Numerics;
using SimRacingSdk.Ams2.SharedMemory.Enums;
using SimRacingSdk.Ams2.SharedMemory.Messages;

namespace SimRacingSdk.Ams2.SharedMemory.Models;

public record SharedMemoryData
{
    internal SharedMemoryData() { }

    internal SharedMemoryData(SharedMemoryPage sharedMemoryPage)
    {
        this.Version = sharedMemoryPage.Version;
        this.BuildVersion = sharedMemoryPage.BuildVersionNumber;
        this.GameState = (Ams2GameState)sharedMemoryPage.GameState;
        this.SessionState = (Ams2SessionState)sharedMemoryPage.SessionState;
        this.RaceState = (Ams2RaceState)sharedMemoryPage.RaceState;
        this.FocusedParticipantIndex = sharedMemoryPage.FocusedParticipantIndex;
        this.ParticipantCount = sharedMemoryPage.ParticipantCount;
        this.ParticipantInfo = sharedMemoryPage.Participants.Select(p => new Ams2ParticipantInfo(p))
                                               .ToArray();
        this.UnfilteredThrottle = sharedMemoryPage.UnfilteredThrottle;
        this.UnfilteredBrake = sharedMemoryPage.UnfilteredBrake;
        this.UnfilteredSteering = sharedMemoryPage.UnfilteredSteering;
        this.UnfilteredClutch = sharedMemoryPage.UnfilteredClutch;
        this.CarName = sharedMemoryPage.CarName;
        this.CarClassName = sharedMemoryPage.CarClassName;
        this.LapsInEvent = sharedMemoryPage.LapsInEvent;
        this.TrackLocation = sharedMemoryPage.TrackLocation;
        this.TrackLayout = sharedMemoryPage.TrackLayout;
        this.SectorCount = sharedMemoryPage.SectorCount;
        this.IsLapInvalid = sharedMemoryPage.IsLapInvalid;
        this.BestLapTime = TimeSpan.FromMilliseconds(sharedMemoryPage.BestLapTime);
        this.LastLapTime = TimeSpan.FromMilliseconds(sharedMemoryPage.LastLapTime);
        this.CurrentLapTime = TimeSpan.FromMilliseconds(sharedMemoryPage.CurrentLapTime);
        this.SplitTimeAhead = TimeSpan.FromMilliseconds(sharedMemoryPage.SplitTimeAhead);
        this.SplitTimeBehind = TimeSpan.FromMilliseconds(sharedMemoryPage.SplitTimeBehind);
        this.EventTimeRemaining = TimeSpan.FromMilliseconds(sharedMemoryPage.EventTimeRemaining);
        this.PersonalBestLapTime = TimeSpan.FromMilliseconds(sharedMemoryPage.PersonalBestLapTime);
        this.OverallBestLapTime = TimeSpan.FromMilliseconds(sharedMemoryPage.OverallBestLapTime);
        this.CurrentSector1Time = TimeSpan.FromMilliseconds(sharedMemoryPage.CurrentSector1Time);
        this.CurrentSector2Time = TimeSpan.FromMilliseconds(sharedMemoryPage.CurrentSector2Time);
        this.BestSector3Time = TimeSpan.FromMilliseconds(sharedMemoryPage.BestSector3Time);
        this.BestSector1Time = TimeSpan.FromMilliseconds(sharedMemoryPage.BestSector1Time);
        this.BestSector2Time = TimeSpan.FromMilliseconds(sharedMemoryPage.BestSector2Time);
        this.BestSector3Time = TimeSpan.FromMilliseconds(sharedMemoryPage.BestSector3Time);
        this.OverallBestSector1Time = TimeSpan.FromMilliseconds(sharedMemoryPage.OverallBestSector1Time);
        this.OverallBestSector2Time = TimeSpan.FromMilliseconds(sharedMemoryPage.OverallBestSector2Time);
        this.OverallBestSector3Time = TimeSpan.FromMilliseconds(sharedMemoryPage.OverallBestSector3Time);
        this.HighestFlagColor = (Ams2FlagColor)sharedMemoryPage.HighestFlagColour;
        this.HighestFlagReason = (Ams2FlagReason)sharedMemoryPage.HighestFlagReason;
        this.PitMode = (Ams2PitMode)sharedMemoryPage.PitMode;
        this.PitSchedule = (Ams2PitStopSchedule)sharedMemoryPage.PitSchedule;
        this.CarFlags = (Ams2CarFlags)sharedMemoryPage.CarFlags;
        this.OilTempC = sharedMemoryPage.OilTempC;
        this.OilPressureKpa = sharedMemoryPage.OilPressureKpa;
        this.WaterTempC = sharedMemoryPage.WaterTempC;
        this.WaterPressureKpa = sharedMemoryPage.WaterPressureKpa;
        this.FuelPressureKpa = sharedMemoryPage.FuelPressureKpa;
        this.FuelLevel = sharedMemoryPage.FuelLevel;
        this.FuelCapacity = sharedMemoryPage.FuelCapacity;
        this.Speed = sharedMemoryPage.Speed;
        this.Rpm = sharedMemoryPage.Rpm;
        this.MaxRpm = sharedMemoryPage.MaxRpm;
        this.Brake = sharedMemoryPage.Brake;
        this.Throttle = sharedMemoryPage.Throttle;
        this.Clutch = sharedMemoryPage.Clutch;
        this.Steering = sharedMemoryPage.Steering;
        this.Gear = sharedMemoryPage.Gear;
        this.GearCount = sharedMemoryPage.GearCount;
        this.OdometerKm = sharedMemoryPage.OdometerKm;
        this.IsAbsActive = sharedMemoryPage.IsAbsActive;
        this.LastOpponentCollisionIndex = sharedMemoryPage.LastOpponentCollisionIndex;
        this.LastOpponentCollisionMagnitude = sharedMemoryPage.LasOpponentCollisionMagnitude;
        this.IsBoostActive = sharedMemoryPage.IsBoostActive;
        this.Orientation = new Vector3(sharedMemoryPage.Orientation[0],
            sharedMemoryPage.Orientation[1],
            sharedMemoryPage.Orientation[2]);
        this.LocalVelocity = new Vector3(sharedMemoryPage.LocalVelocity[0],
            sharedMemoryPage.LocalVelocity[1],
            sharedMemoryPage.LocalVelocity[2]);
        this.WorldVelocity = new Vector3(sharedMemoryPage.WorldVelocity[0],
            sharedMemoryPage.WorldVelocity[1],
            sharedMemoryPage.WorldVelocity[2]);
        this.AngularVelocity = new Vector3(sharedMemoryPage.AngularVelocity[0],
            sharedMemoryPage.AngularVelocity[1],
            sharedMemoryPage.AngularVelocity[2]);
        this.LocalAcceleration = new Vector3(sharedMemoryPage.LocalAcceleration[0],
            sharedMemoryPage.LocalAcceleration[1],
            sharedMemoryPage.LocalAcceleration[2]);
        this.WorldAcceleration = new Vector3(sharedMemoryPage.WorldAcceleration[0],
            sharedMemoryPage.WorldAcceleration[1],
            sharedMemoryPage.WorldAcceleration[2]);
        this.ExtentsCenter = new Vector3(sharedMemoryPage.ExtentsCenter[0],
            sharedMemoryPage.ExtentsCenter[1],
            sharedMemoryPage.ExtentsCenter[2]);
        this.TyreFlags = new Ams2WheelMetric<Ams2TyreFlag>((Ams2TyreFlag)sharedMemoryPage.TyreFlags[0],
            (Ams2TyreFlag)sharedMemoryPage.TyreFlags[1],
            (Ams2TyreFlag)sharedMemoryPage.TyreFlags[2],
            (Ams2TyreFlag)sharedMemoryPage.TyreFlags[3]);
        this.Terrain = new Ams2WheelMetric<Ams2TerrainMaterial>((Ams2TerrainMaterial)sharedMemoryPage.Terrain[0],
            (Ams2TerrainMaterial)sharedMemoryPage.Terrain[1],
            (Ams2TerrainMaterial)sharedMemoryPage.Terrain[2],
            (Ams2TerrainMaterial)sharedMemoryPage.Terrain[3]);
        this.WheelLocalPositionY = new Ams2WheelMetric<float>(sharedMemoryPage.WheelLocalPositionY[0],
            sharedMemoryPage.WheelLocalPositionY[1],
            sharedMemoryPage.WheelLocalPositionY[2],
            sharedMemoryPage.WheelLocalPositionY[3]);
        this.TyreRps = new Ams2WheelMetric<float>(sharedMemoryPage.TyreRps[0],
            sharedMemoryPage.TyreRps[1],
            sharedMemoryPage.TyreRps[2],
            sharedMemoryPage.TyreRps[3]);
        this.TyreSlipSpeed = new Ams2WheelMetric<float>(sharedMemoryPage.TyreSlipSpeed[0],
            sharedMemoryPage.TyreSlipSpeed[1],
            sharedMemoryPage.TyreSlipSpeed[2],
            sharedMemoryPage.TyreSlipSpeed[3]);
        this.TyreTempC = new Ams2WheelMetric<float>(sharedMemoryPage.TyreTempC[0],
            sharedMemoryPage.TyreTempC[1],
            sharedMemoryPage.TyreTempC[2],
            sharedMemoryPage.TyreTempC[3]);
        this.TyreGrip = new Ams2WheelMetric<float>(sharedMemoryPage.TyreGrip[0],
            sharedMemoryPage.TyreGrip[1],
            sharedMemoryPage.TyreGrip[2],
            sharedMemoryPage.TyreGrip[3]);
        this.TyreHeightAboveGround = new Ams2WheelMetric<float>(sharedMemoryPage.TyreHeightAboveGround[0],
            sharedMemoryPage.TyreHeightAboveGround[1],
            sharedMemoryPage.TyreHeightAboveGround[2],
            sharedMemoryPage.TyreHeightAboveGround[3]);
        this.TyreWear = new Ams2WheelMetric<float>(sharedMemoryPage.TyreWear[0],
            sharedMemoryPage.TyreWear[1],
            sharedMemoryPage.TyreWear[2],
            sharedMemoryPage.TyreWear[3]);
        this.BrakeDamage = new Ams2WheelMetric<float>(sharedMemoryPage.BrakeDamage[0],
            sharedMemoryPage.BrakeDamage[1],
            sharedMemoryPage.BrakeDamage[2],
            sharedMemoryPage.BrakeDamage[3]);
        this.SuspensionDamage = new Ams2WheelMetric<float>(sharedMemoryPage.SuspensionDamage[0],
            sharedMemoryPage.SuspensionDamage[1],
            sharedMemoryPage.SuspensionDamage[2],
            sharedMemoryPage.SuspensionDamage[3]);
        this.BrakeTempC = new Ams2WheelMetric<float>(sharedMemoryPage.BrakeTempC[0],
            sharedMemoryPage.BrakeTempC[1],
            sharedMemoryPage.BrakeTempC[2],
            sharedMemoryPage.BrakeTempC[3]);
        this.TyreTreadTempK = new Ams2WheelMetric<float>(sharedMemoryPage.TyreTreadTempK[0],
            sharedMemoryPage.TyreTreadTempK[1],
            sharedMemoryPage.TyreTreadTempK[2],
            sharedMemoryPage.TyreTreadTempK[3]);
        this.TyreLayerTempK = new Ams2WheelMetric<float>(sharedMemoryPage.TyreLayerTempK[0],
            sharedMemoryPage.TyreLayerTempK[1],
            sharedMemoryPage.TyreLayerTempK[2],
            sharedMemoryPage.TyreLayerTempK[3]);
        this.TyreCarcassTempK = new Ams2WheelMetric<float>(sharedMemoryPage.TyreCarcassTempK[0],
            sharedMemoryPage.TyreCarcassTempK[1],
            sharedMemoryPage.TyreCarcassTempK[2],
            sharedMemoryPage.TyreCarcassTempK[3]);
        this.TyreRimTempK = new Ams2WheelMetric<float>(sharedMemoryPage.TyreRimTempK[0],
            sharedMemoryPage.TyreRimTempK[1],
            sharedMemoryPage.TyreRimTempK[2],
            sharedMemoryPage.TyreRimTempK[3]);
        this.TyreInternalAirTempK = new Ams2WheelMetric<float>(sharedMemoryPage.TyreInternalAirTempK[0],
            sharedMemoryPage.TyreInternalAirTempK[1],
            sharedMemoryPage.TyreInternalAirTempK[2],
            sharedMemoryPage.TyreInternalAirTempK[3]);
        this.CrashDamageState = (Ams2CrashDamageState)sharedMemoryPage.CrashState;
        this.AeroDamage = sharedMemoryPage.AeroDamage;
        this.EngineDamage = sharedMemoryPage.EngineDamage;
        this.AmbientTempC = sharedMemoryPage.AmbientTempC;
        this.TrackTempC = sharedMemoryPage.TrackTempC;
        this.RainDensity = sharedMemoryPage.RainDensity;
        this.WindDirectionX = sharedMemoryPage.WindDirectionX;
        this.WindDirectionY = sharedMemoryPage.WindDirectionY;
        this.CloudBrightness = sharedMemoryPage.CloudBrightness;
        this.SequenceNumber = sharedMemoryPage.SequenceNumber;
        this.WheelLocalPositionY = new Ams2WheelMetric<float>(sharedMemoryPage.WheelLocalPositionY[0],
            sharedMemoryPage.WheelLocalPositionY[1],
            sharedMemoryPage.WheelLocalPositionY[2],
            sharedMemoryPage.WheelLocalPositionY[3]);
        this.SuspensionTravel = new Ams2WheelMetric<float>(sharedMemoryPage.SuspensionTravel[0],
            sharedMemoryPage.SuspensionTravel[1],
            sharedMemoryPage.SuspensionTravel[2],
            sharedMemoryPage.SuspensionTravel[3]);
        this.SuspensionVelocity = new Ams2WheelMetric<float>(sharedMemoryPage.SuspensionVelocity[0],
            sharedMemoryPage.SuspensionVelocity[1],
            sharedMemoryPage.SuspensionVelocity[2],
            sharedMemoryPage.SuspensionVelocity[3]);
        this.AirPressure = new Ams2WheelMetric<float>(sharedMemoryPage.AirPressure[0],
            sharedMemoryPage.AirPressure[1],
            sharedMemoryPage.AirPressure[2],
            sharedMemoryPage.AirPressure[3]);
        this.EngineSpeed = sharedMemoryPage.EngineSpeed;
        this.EngineTorque = sharedMemoryPage.EngineTorque;
        this.FrontWing = sharedMemoryPage.Wings[0];
        this.RearWing = sharedMemoryPage.Wings[1];
        this.HandBrake = sharedMemoryPage.HandBrake;
        this.CurrentSector1Times = sharedMemoryPage.CurrentSector1Times;
        this.CurrentSector2Times = sharedMemoryPage.CurrentSector2Times;
        this.CurrentSector3Times = sharedMemoryPage.CurrentSector3Times;
        this.BestSector1Times = sharedMemoryPage.BestSector1Times;
        this.BestSector2Times = sharedMemoryPage.BestSector2Times;
        this.BestSector3Times = sharedMemoryPage.BestSector3Times;
        this.LastLapTimes = sharedMemoryPage.LastLapTimes;
        this.IsLapInvalidated = sharedMemoryPage.IsLapInvalidated.Select(b => b != 0)
                                                .ToArray();
        this.RaceStates = sharedMemoryPage.RaceStates.Select(s => (Ams2RaceState)s)
                                          .ToArray();
        this.PitModes = sharedMemoryPage.RaceStates.Select(m => (Ams2PitMode)m)
                                        .ToArray();
        this.Orientations = sharedMemoryPage.Orientations.Select(o => new Vector3(o[0], o[1], o[2]))
                                            .ToArray();
        this.Speeds = sharedMemoryPage.Speeds;
        this.CarNames = sharedMemoryPage.CarNames;
        this.CarClassNames = sharedMemoryPage.CarClassNames;
        this.PitStopEnforcedOnLap = sharedMemoryPage.PitStopEnforcedOnLap;
        this.TranslatedTrackLocation = sharedMemoryPage.TranslatedTrackLocation;
        this.TranslatedTrackLayout = sharedMemoryPage.TranslatedTrackLayout;
        this.BrakeBias = sharedMemoryPage.BrakeBias;
        this.TurboBoostPressure = sharedMemoryPage.TurboBoostPressure;
        this.TyreCompound = new Ams2WheelMetric<string>(sharedMemoryPage.TyreCompound[0],
            sharedMemoryPage.TyreCompound[1],
            sharedMemoryPage.TyreCompound[2],
            sharedMemoryPage.TyreCompound[3]);
        this.PitSchedules = sharedMemoryPage.PitSchedules.Select(s => (Ams2PitStopSchedule)s)
                                            .ToArray();
        this.HighestFlagColors = sharedMemoryPage.HighestFlagColours.Select(c => (Ams2FlagColor)c)
                                                 .ToArray();
        this.HighestFlagReasons = sharedMemoryPage.HighestFlagReasons.Select(r => (Ams2FlagReason)r)
                                                  .ToArray();
        this.Nationalities = sharedMemoryPage.Nationalities;
        this.SnowDensity = sharedMemoryPage.SnowDensity;
        this.SessionAdditionalLaps = sharedMemoryPage.SessionAdditionalLaps;
        this.TyreTempLeftC = new Ams2WheelMetric<float>(sharedMemoryPage.TyreTempLeftC[0],
            sharedMemoryPage.TyreTempLeftC[1],
            sharedMemoryPage.TyreTempLeftC[2],
            sharedMemoryPage.TyreTempLeftC[3]);
        this.TyreTempCenterC = new Ams2WheelMetric<float>(sharedMemoryPage.TyreTempCenterC[0],
            sharedMemoryPage.TyreTempCenterC[1],
            sharedMemoryPage.TyreTempCenterC[2],
            sharedMemoryPage.TyreTempCenterC[3]);
        this.TyreTempRightC = new Ams2WheelMetric<float>(sharedMemoryPage.TyreTempRightC[0],
            sharedMemoryPage.TyreTempRightC[1],
            sharedMemoryPage.TyreTempRightC[2],
            sharedMemoryPage.TyreTempRightC[3]);
        this.DrsState = (Ams2DrsState)sharedMemoryPage.DrsState;
        this.RideHeight = new Ams2WheelMetric<float>(sharedMemoryPage.RideHeight[0],
            sharedMemoryPage.RideHeight[1],
            sharedMemoryPage.RideHeight[2],
            sharedMemoryPage.RideHeight[3]);
        this.JoyPad0Mask = sharedMemoryPage.JoyPad0;
        this.DPadMask = sharedMemoryPage.DPad;
        this.AbsSetting = sharedMemoryPage.AbsSetting;
        this.TCSetting = sharedMemoryPage.TCSetting;
        this.ErsDeploymentMode = (Ams2ErsDeploymentMode)sharedMemoryPage.ErsDeploymentMode;
        this.IsErsAutoModeEnabled = sharedMemoryPage.IsErsAutoModeEnabled;
        this.ClutchTempK = sharedMemoryPage.ClutchTempK;
        this.ClutchWear = sharedMemoryPage.ClutchWear;
        this.IsClutchOverheated = sharedMemoryPage.IsClutchOverheated;
        this.IsClutchSlipping = sharedMemoryPage.IsClutchSlipping;
        this.YellowFlagState = (Ams2YellowFlagState)sharedMemoryPage.YellowFlagState;
        this.IsPrivateSession = sharedMemoryPage.IsPrivateSession;
        this.LaunchStage = (Ams2LaunchStage)sharedMemoryPage.LaunchStage;
    }

    public int AbsSetting { get; init; }
    public float AeroDamage { get; init; }
    public Ams2WheelMetric<float> AirPressure { get; init; }
    public float AmbientTempC { get; init; }
    public Vector3 AngularVelocity { get; init; }
    public TimeSpan BestLapTime { get; init; }
    public TimeSpan BestSector1Time { get; init; }
    public float[] BestSector1Times { get; init; }
    public TimeSpan BestSector2Time { get; init; }
    public float[] BestSector2Times { get; init; }
    public TimeSpan BestSector3Time { get; init; }
    public float[] BestSector3Times { get; init; }
    public float Brake { get; init; }
    public float BrakeBias { get; init; }
    public Ams2WheelMetric<float> BrakeDamage { get; init; }
    public Ams2WheelMetric<float> BrakeTempC { get; init; }
    public uint BuildVersion { get; init; }
    public string CarClassName { get; init; }
    public string[] CarClassNames { get; init; }
    public Ams2CarFlags CarFlags { get; init; }
    public string CarName { get; init; }
    public string[] CarNames { get; init; }
    public float CloudBrightness { get; init; }
    public float Clutch { get; init; }
    public float ClutchTempK { get; init; }
    public float ClutchWear { get; init; }
    public Ams2CrashDamageState CrashDamageState { get; init; }
    public TimeSpan CurrentLapTime { get; init; }
    public TimeSpan CurrentSector1Time { get; init; }
    public float[] CurrentSector1Times { get; init; }
    public TimeSpan CurrentSector2Time { get; init; }
    public float[] CurrentSector2Times { get; init; }
    public TimeSpan CurrentSector3Time { get; init; }
    public float[] CurrentSector3Times { get; init; }
    public uint DPadMask { get; init; }
    public Ams2DrsState DrsState { get; init; }
    public float EngineDamage { get; init; }
    public float EngineSpeed { get; init; }
    public float EngineTorque { get; init; }
    public Ams2ErsDeploymentMode ErsDeploymentMode { get; init; }
    public TimeSpan EventTimeRemaining { get; init; }
    public Vector3 ExtentsCenter { get; init; }
    public int FocusedParticipantIndex { get; init; }
    public float FrontWing { get; init; }
    public float FuelCapacity { get; init; }
    public float FuelLevel { get; init; }
    public float FuelPressureKpa { get; init; }
    public Ams2GameState GameState { get; init; }
    public int Gear { get; init; }
    public int GearCount { get; init; }
    public float HandBrake { get; init; }
    public Ams2FlagColor HighestFlagColor { get; init; }
    public Ams2FlagColor[] HighestFlagColors { get; init; }
    public Ams2FlagReason HighestFlagReason { get; init; }
    public Ams2FlagReason[] HighestFlagReasons { get; init; }
    public bool IsAbsActive { get; init; }
    public bool IsBoostActive { get; init; }
    public bool IsClutchOverheated { get; init; }
    public bool IsClutchSlipping { get; init; }
    public bool IsErsAutoModeEnabled { get; init; }
    public bool IsLapInvalid { get; init; }
    public bool[] IsLapInvalidated { get; init; }
    public bool IsPrivateSession { get; init; }
    public uint JoyPad0Mask { get; init; }
    public uint LapsInEvent { get; init; }
    public TimeSpan LastLapTime { get; init; }
    public float[] LastLapTimes { get; init; }
    public int LastOpponentCollisionIndex { get; init; }
    public float LastOpponentCollisionMagnitude { get; init; }
    public Ams2LaunchStage LaunchStage { get; init; }
    public Vector3 LocalAcceleration { get; init; }
    public Vector3 LocalVelocity { get; init; }
    public float MaxRpm { get; init; }
    public uint[] Nationalities { get; init; }
    public float OdometerKm { get; init; }
    public float OilPressureKpa { get; init; }
    public float OilTempC { get; init; }
    public Vector3 Orientation { get; init; }
    public Vector3[] Orientations { get; init; }
    public TimeSpan OverallBestLapTime { get; init; }
    public TimeSpan OverallBestSector1Time { get; init; }
    public TimeSpan OverallBestSector2Time { get; init; }
    public TimeSpan OverallBestSector3Time { get; init; }
    public int ParticipantCount { get; init; }
    public Ams2ParticipantInfo[] ParticipantInfo { get; init; }
    public TimeSpan PersonalBestLapTime { get; init; }
    public Ams2PitMode PitMode { get; init; }
    public Ams2PitMode[] PitModes { get; init; }
    public Ams2PitStopSchedule PitSchedule { get; init; }
    public Ams2PitStopSchedule[] PitSchedules { get; init; }
    public int PitStopEnforcedOnLap { get; init; }
    public Ams2RaceState RaceState { get; init; }
    public Ams2RaceState[] RaceStates { get; init; }
    public float RainDensity { get; init; }
    public float RearWing { get; init; }
    public Ams2WheelMetric<float> RideHeight { get; init; }
    public float Rpm { get; init; }
    public int SectorCount { get; init; }
    public float SequenceNumber { get; init; }
    public int SessionAdditionalLaps { get; init; }
    public Ams2SessionState SessionState { get; init; }
    public float SnowDensity { get; init; }
    public float Speed { get; init; }
    public float[] Speeds { get; init; }
    public TimeSpan SplitTimeAhead { get; init; }
    public TimeSpan SplitTimeBehind { get; init; }
    public float Steering { get; init; }
    public Ams2WheelMetric<float> SuspensionDamage { get; init; }
    public Ams2WheelMetric<float> SuspensionTravel { get; init; }
    public Ams2WheelMetric<float> SuspensionVelocity { get; init; }
    public int TCSetting { get; init; }
    public Ams2WheelMetric<Ams2TerrainMaterial> Terrain { get; init; }
    public float Throttle { get; init; }
    public string TrackLayout { get; init; }
    public string TrackLocation { get; init; }
    public float TrackTempC { get; init; }
    public string TranslatedTrackLayout { get; init; }
    public string TranslatedTrackLocation { get; init; }
    public float TurboBoostPressure { get; init; }
    public Ams2WheelMetric<float> TyreCarcassTempK { get; init; }
    public Ams2WheelMetric<string> TyreCompound { get; init; }
    public Ams2WheelMetric<Ams2TyreFlag> TyreFlags { get; init; }
    public Ams2WheelMetric<float> TyreGrip { get; init; }
    public Ams2WheelMetric<float> TyreHeightAboveGround { get; init; }
    public Ams2WheelMetric<float> TyreInternalAirTempK { get; init; }
    public Ams2WheelMetric<float> TyreLayerTempK { get; init; }
    public Ams2WheelMetric<float> TyreRimTempK { get; init; }
    public Ams2WheelMetric<float> TyreRps { get; init; }
    public Ams2WheelMetric<float> TyreSlipSpeed { get; init; }
    public Ams2WheelMetric<float> TyreTempC { get; init; }
    public Ams2WheelMetric<float> TyreTempCenterC { get; init; }
    public Ams2WheelMetric<float> TyreTempLeftC { get; init; }
    public Ams2WheelMetric<float> TyreTempRightC { get; init; }
    public Ams2WheelMetric<float> TyreTreadTempK { get; init; }
    public Ams2WheelMetric<float> TyreWear { get; init; }
    public float UnfilteredBrake { get; init; }
    public float UnfilteredClutch { get; init; }
    public float UnfilteredSteering { get; init; }
    public float UnfilteredThrottle { get; init; }
    public uint Version { get; init; }
    public float WaterPressureKpa { get; init; }
    public float WaterTempC { get; init; }
    public Ams2WheelMetric<float> WheelLocalPositionY { get; init; }
    public float WindDirectionX { get; init; }
    public float WindDirectionY { get; init; }
    public Vector3 WorldAcceleration { get; init; }
    public Vector3 WorldVelocity { get; init; }
    public Ams2YellowFlagState YellowFlagState { get; init; }


    public List<Ams2Participant> GetConsolidatedParticipants()
    {
        var result = new List<Ams2Participant>();

        for(var i = 0; i < this.ParticipantCount; i++)
        {
            var participantInfo = this.ParticipantInfo[i];
            var participant = new Ams2Participant
            {
                BestSector1Time = TimeSpan.FromMilliseconds(this.BestSector1Times[i]),
                BestSector2Time = TimeSpan.FromMilliseconds(this.BestSector2Times[i]),
                BestSector3Time = TimeSpan.FromMilliseconds(this.BestSector3Times[i]),
                CarClassName = this.CarClassNames[i],
                CarName = this.CarNames[i],
                CurrentLap = participantInfo.CurrentLap,
                CurrentSector = participantInfo.CurrentSector,
                CurrentSector1Time = TimeSpan.FromMilliseconds(this.CurrentSector1Times[i]),
                CurrentSector2Time = TimeSpan.FromMilliseconds(this.CurrentSector2Times[i]),
                CurrentSector3Time = TimeSpan.FromMilliseconds(this.CurrentSector3Times[i]),
                DistanceIntoCurrentLap = participantInfo.DistanceIntoCurrentLap,
                HighestFlagColor = this.HighestFlagColors[i],
                HighestFlagReason = this.HighestFlagReasons[i],
                IsActive = participantInfo.IsActive,
                IsLapInvalid = this.IsLapInvalidated[i],
                LapsCompleted = participantInfo.LapsCompleted,
                LastLapTime = TimeSpan.FromMilliseconds(this.LastLapTimes[i]),
                Name = participantInfo.Name,
                Nationality = this.Nationalities[i],
                Orientation = this.Orientations[i],
                PitMode = this.PitModes[i],
                PitSchedule = this.PitSchedules[i],
                RacePosition = participantInfo.RacePosition,
                RaceState = this.RaceStates[i],
                Speed = this.Speeds[i],
                WorldPosition = participantInfo.WorldPosition,
            };

            result.Add(participant);
        }

        return result;
    }

    public Ams2Participant GetPlayer()
    {
        var participantInfo = this.ParticipantInfo[this.FocusedParticipantIndex];
        return new Ams2Participant
        {
            BestSector1Time = this.BestSector1Time,
            BestSector2Time = this.BestSector2Time,
            BestSector3Time = this.BestSector3Time,
            CarClassName = this.CarClassName,
            CarName = this.CarName,
            CurrentLap = participantInfo.CurrentLap,
            CurrentSector = participantInfo.CurrentSector,
            CurrentSector1Time = this.CurrentSector1Time,
            CurrentSector2Time = this.CurrentSector2Time,
            CurrentSector3Time = this.CurrentSector3Time,
            DistanceIntoCurrentLap = participantInfo.DistanceIntoCurrentLap,
            HighestFlagColor = this.HighestFlagColor,
            HighestFlagReason = this.HighestFlagReason,
            IsActive = participantInfo.IsActive,
            IsLapInvalid = this.IsLapInvalid,
            LapsCompleted = participantInfo.LapsCompleted,
            LastLapTime = this.LastLapTime,
            Name = participantInfo.Name,
            Nationality = this.Nationalities[this.FocusedParticipantIndex],
            Orientation = this.Orientation,
            PitMode = this.PitMode,
            PitSchedule = this.PitSchedule,
            RacePosition = participantInfo.RacePosition,
            RaceState = this.RaceState,
            Speed = this.Speed,
            WorldPosition = participantInfo.WorldPosition,
        };
    }

    public Ams2VehicleState GetVehicleState()
    {
        return new Ams2VehicleState
        {
            AbsSetting = this.AbsSetting,
            AeroDamage = this.AeroDamage,
            AirPressure = this.AirPressure,
            AngularVelocity = this.AngularVelocity,
            Brake = this.Brake,
            BrakeBias = this.BrakeBias,
            BrakeTempC = this.BrakeTempC,
            BrakeDamage = this.BrakeDamage,
            CarFlags = this.CarFlags,
            Class = this.CarClassName,
            Clutch = this.Clutch,
            ClutchTempK = this.ClutchTempK,
            ClutchWear = this.ClutchWear,
            CrashDamageState = this.CrashDamageState,
            EngineDamage = this.EngineDamage,
            EngineSpeed = this.EngineSpeed,
            EngineTorque = this.EngineTorque,
            ExtentsCenter = this.ExtentsCenter,
            ErsDeploymentMode = this.ErsDeploymentMode,
            FrontWing = this.FrontWing,
            FuelCapacity = this.FuelCapacity,
            FuelLevel = this.FuelLevel,
            FuelPressureKpa = this.FuelPressureKpa,
            Gear = this.Gear,
            GearCount = this.GearCount,
            HandBrake = this.HandBrake,
            IsAbsActive = this.IsAbsActive,
            IsBoostActive = this.IsBoostActive,
            IsClutchSlipping = this.IsClutchSlipping,
            IsClutchOverheated = this.IsClutchOverheated,
            IsErsAutoModeEnabled = this.IsErsAutoModeEnabled,
            LaunchStage = this.LaunchStage,
            LocalAcceleration = this.LocalAcceleration,
            LocalVelocity = this.LocalVelocity,
            MaxRpm = this.MaxRpm,
            Name = this.CarName,
            OdometerKm = this.OdometerKm,
            OilPressureKpa = this.OilPressureKpa,
            OilTempC = this.OilTempC,
            Orientation = this.Orientation,
            RearWing = this.RearWing,
            RideHeight = this.RideHeight,
            Rpm = this.Rpm,
            Speed = this.Speed,
            Steering = this.Steering,
            TCSetting = this.TCSetting,
            Throttle = this.Throttle,
            TyreCarcassTempK = this.TyreCarcassTempK,
            TyreCompound = this.TyreCompound,
            TyreFlags = this.TyreFlags,
            TyreGrip = this.TyreGrip,
            TyreHeightAboveGround = this.TyreHeightAboveGround,
            TyreInternalAirTempK = this.TyreInternalAirTempK,
            TyreLayerTempK = this.TyreLayerTempK,
            TyreRimTempK = this.TyreRimTempK,
            TyreRps = this.TyreRps,
            TyreSlipSpeed = this.TyreSlipSpeed,
            TyreTempC = this.TyreTempC,
            TyreTempCenterC = this.TyreTempCenterC,
            TyreTempLeftC = this.TyreTempLeftC,
            TyreTempRightC = this.TyreTempRightC,
            TyreTreadTempK = this.TyreTreadTempK,
            TyreWear = this.TyreWear,
            UnfilteredBrake = this.UnfilteredBrake,
            UnfilteredClutch = this.UnfilteredClutch,
            UnfilteredSteering = this.UnfilteredSteering,
            UnfilteredThrottle = this.UnfilteredThrottle,
            WaterPressureKpa = this.WaterPressureKpa,
            WaterTempC = this.WaterTempC,
            WheelLocalPositionY = this.WheelLocalPositionY,
            WorldAcceleration = this.WorldAcceleration,
            WorldVelocity = this.WorldVelocity
        };
    }
}