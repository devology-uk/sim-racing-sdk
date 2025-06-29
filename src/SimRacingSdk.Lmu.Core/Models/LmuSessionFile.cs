using System.Xml.Linq;

namespace SimRacingSdk.Lmu.Core.Models;

public class LmuSessionFile
{
    public int ClientFuelVisible { get; set; }
    public LmuConnectionType? ConnectionType { get; set; }
    public int DamageMultiplier { get; set; }
    public long DateTimeValue { get; set; }
    public int Dedicated { get; set; }
    public int FixedSetup { get; set; }
    public int FixedUpgrades { get; set; }
    public int FreeSettings { get; set; }
    public int FuelMultiplier { get; set; }
    public string? GameVersion { get; set; }
    public int LimitedTyres { get; set; }
    public int MechanicalFailRate { get; set; }
    public int ParcFerme { get; set; }
    public string? PlayerFile { get; set; }
    public int RaceLaps { get; set; }
    public int RaceTime { get; set; }
    public string? ServerName { get; set; }
    public LmuDrivingSession? Session { get; set; }
    public string? Setting { get; set; }
    public string? TimeString { get; set; }
    public int TireMultiplier { get; set; }
    public int TireWarmers { get; set; }
    public string? TrackCourse { get; set; }
    public string? TrackData { get; set; }
    public string? TrackEvent { get; set; }
    public double TrackLength { get; set; }
    public string? TrackVenue { get; set; }
    public string? VehiclesAllowed { get; set; }

    public static LmuSessionFile Load(string filePath)
    {
        var document = XElement.Load(filePath);
        var raceResultsElement = document.Element("RaceResults");

        if(raceResultsElement == null)
        {
            throw (new ArgumentException($"{filePath} is not a valid LMU result file.", nameof(filePath)));
        }

        var result = InitialiseResult(raceResultsElement);

        AddConnectionType(raceResultsElement, result);
        AddSession(raceResultsElement, result);

        return result;
    }

    private static void AddConnectionType(XElement raceResultsElement, LmuSessionFile result)
    {
        var connectionTypeElement = raceResultsElement.Element("ConnectionType");
        if(connectionTypeElement != null)
        {
            result.ConnectionType = new LmuConnectionType
            {
                Upload = GetValue<int>(connectionTypeElement.Attribute("upload")
                                                            ?.Value),
                Download = GetValue<int>(connectionTypeElement.Attribute("download")
                                                              ?.Value)
            };
        }
    }

    private static void AddEventStream(XElement element, LmuDrivingSession session)
    {
        var streamElement = element.Element("Stream");
        if(streamElement == null)
        {
            return;
        }

        foreach(var eventElement in streamElement.Descendants())
        {
            switch(eventElement.Name.LocalName)
            {
                case "Incident":
                    session.Stream.Add(new LmuIncidentStreamEvent(GetValue<double>(eventElement.Attribute("et")
                            ?.Value),
                        eventElement.Value));
                    break;
                case "Score":
                    session.Stream.Add(new LmuScoreStreamEvent(GetValue<double>(eventElement.Attribute("et")
                            ?.Value),
                        eventElement.Value));
                    break;
                case "Sector":
                    session.Stream.Add(new LmuSectorStreamEvent(GetValue<double>(eventElement.Attribute("et")
                            ?.Value),
                        eventElement.Value,
                        eventElement.Element("Driver")
                                    ?.Value ?? string.Empty,
                        GetValue<int>(eventElement.Element("ID")
                                                  ?.Value),
                        GetValue<int>(eventElement.Element("Sector")
                                                  ?.Value),
                        eventElement.Element("Class")
                                    ?.Value ?? string.Empty));
                    break;
                case "Sent":
                    session.Stream.Add(new LmuSentStreamEvent(GetValue<double>(eventElement.Attribute("et")
                            ?.Value),
                        eventElement.Value));
                    break;
                case "TrackLimits":
                    session.Stream.Add(new LmuTrackLimitsStreamEvent(
                        GetValue<double>(eventElement.Attribute("et")
                            ?.Value),
                        eventElement.Value,
                        eventElement.Element("Driver")?.Value ?? string.Empty,
                        GetValue<int>(eventElement.Element("ID")
                            ?.Value),
                        GetValue<int>(eventElement.Element("Lap")
                            ?.Value),
                        GetValue<int>(eventElement.Element("WarningPoints")
                            ?.Value),
                        GetValue<int>(eventElement.Element("CurrentPoints")
                            ?.Value),
                        GetValue<int>(eventElement.Element("Resolution")
                            ?.Value)));
                    break;
                default:
                    break;
            }
        }
    }

    private static void AddSession(XElement element, LmuSessionFile result)
    {
        var sessionElement = GetSessionElement(element);
        if(sessionElement == null)
        {
            throw (new ArgumentException("The result file does not contain a valid session element"));
        }

        var session = new LmuDrivingSession
        {
            SessionType = GetSessionType(sessionElement),
            SessionName = sessionElement.Name.LocalName,
            DateTimeValue = GetValue<int>(sessionElement.Element("DateTime")
                                                        ?.Value),
            TimeString = sessionElement.Element("TimeString")
                                       ?.Value,
            Laps = GetValue<int>(sessionElement.Element("Laps")
                                               ?.Value),
            Minutes = GetValue<int>(sessionElement.Element("Minutes")
                                                  ?.Value),
            FormationAndStart = GetValue<int>(sessionElement.Element("FormationAndStart")
                                                            ?.Value),
            MostLapsCompleted = GetValue<int>(sessionElement.Element("MostLapsCompleted")
                                                            ?.Value)
        };

        AddEventStream(element, session);
        AddDrivers(element, session);

        result.Session = session;
    }

    private static void AddDrivers(XElement element, LmuDrivingSession session)
    {
        var driverElements = element.Elements("Driver");
        foreach(var driverElement in driverElements)
        {
            var driver = new LmuDriver
            {
                Name = driverElement.Element("Name")?.Value,
                Connected = GetValue<int>(driverElement.Element("Connected")
                                                        ?.Value),
                VehicleFile = driverElement.Element("VehicleFile")
                                                ?.Value,
                UpgradeCode = driverElement.Element("UpgradeCode")
                                                ?.Value,
                VehicleName = driverElement.Element("VehicleName")
                                                ?.Value,
                Category = driverElement.Element("Category")
                                                ?.Value,
                CarType = driverElement.Element("CarType")
                                                ?.Value,
                CarClass = driverElement.Element("CarClass")
                                                ?.Value,
                CarNumber = GetValue<int>(driverElement.Element("CarNumber")
                                                        ?.Value),
                TeamName = driverElement.Element("TeamName")
                                                ?.Value,
                IsPlayer = GetValue<int>(driverElement.Element("IsPlayer")
                                                            ?.Value),
                ServerScored = GetValue<int>(driverElement.Element("ServerScored")
                                                            ?.Value),
                GridPosition = GetValue<int>(driverElement.Element("GridPos")
                                                            ?.Value),
                Position = GetValue<int>(driverElement.Element("Position")
                                                            ?.Value),
                ClassGridPosition = GetValue<int>(driverElement.Element("ClassGridPos")
                                                            ?.Value),
                ClassPosition = GetValue<int>(driverElement.Element("ClassPosition")
                                                            ?.Value),
                LapRankIncludingDiscos = GetValue<int>(driverElement.Element("LapRankIncludingDiscos")?.Value),
                BestLap = GetValue<double>(driverElement.Element("BestLap")
                                                            ?.Value),
                FinishTime = GetValue<double>(driverElement.Element("FinishTime")
                                                            ?.Value),
                LapCount = GetValue<int>(driverElement.Element("Laps")
                                                            ?.Value),
                PitStops = GetValue<int>(driverElement.Element("PitStops")?.Value),
                FinishStatus = driverElement.Element("FinishStatus")?.Value
            };

            AddDriverControlAndAids(driverElement, driver);
            AddDriverLaps(driverElement, driver);
            session.Drivers.Add(driver);
        }
    }

    private static void AddDriverLaps(XElement driverElement, LmuDriver driver)
    {
        var lapElements = driverElement.Elements("Lap");
        foreach(var lapElement in lapElements)
        {
            var lap = new LmuDriverLap
            {
                Number = GetValue<int>(lapElement.Attribute("numb")
                                                            ?.Value),
                EventTiming = GetValue<double>(lapElement.Attribute("et")
                                                            ?.Value),
                Sector1Time = GetValue<double>(lapElement.Attribute("s1")?.Value),
                Sector2Time = GetValue<double>(lapElement.Attribute("s2")?.Value),
                Sector3Time = GetValue<double>(lapElement.Attribute("s3")?.Value),
                TopSpeed = GetValue<double>(lapElement.Attribute("topspeed")?.Value),
                Fuel = GetValue<double>(lapElement.Attribute("fuel")?.Value),
                FuelUsed = GetValue<double>(lapElement.Attribute("fuelUsed")?.Value),
                VirtualEnergy = GetValue<double>(lapElement.Attribute("ve")?.Value),
                VirtualEnergyUsed = GetValue<double>(lapElement.Attribute("veUsed")?.Value),
                TyreWearFrontLeft = GetValue<double>(lapElement.Attribute("twfl")?.Value),
                TyreWearFrontRight = GetValue<double>(lapElement.Attribute("twfr")?.Value),
                TyreWearRearLeft = GetValue<double>(lapElement.Attribute("twrl")?.Value),
                TyreWearRearRight = GetValue<double>(lapElement.Attribute("twrr")?.Value),
                TyreFrontCompound = lapElement.Attribute("fcompound")?.Value,
                TyreRearCompound = lapElement.Attribute("rcompound")?.Value,
                TyreFrontLeftCompound = lapElement.Attribute("FL")?.Value,
                TyreFrontRightCompound = lapElement.Attribute("FR")
                                                  ?.Value,
                TyreRearLeftCompound = lapElement.Attribute("RL")?.Value,
                TyreRearRightCompound = lapElement.Attribute("RR")
                                                 ?.Value,
                LapTime = GetValue<double>(lapElement.Value)

            };
            driver.Laps.Add(lap);
        }
    }

    private static void AddDriverControlAndAids(XElement driverElement, LmuDriver driver)
    {
        var controlAndAidsElement = driverElement.Element("ControlAndAids");
        if(controlAndAidsElement != null)
        {
            driver.ControlAndAids = new LmuControlAndAids(
                GetValue<int>(controlAndAidsElement.Attribute("startLap")
                                                   ?.Value),
                GetValue<int>(controlAndAidsElement.Attribute("endLap")
                                                   ?.Value),
                controlAndAidsElement.Value);
        }
    }

    private static XElement? GetSessionElement(XElement element)
    {
        var sessionElement = element.Element("Race");
        if(sessionElement != null)
        {
            return sessionElement;
        }

        return element.Element("Qualifying") ?? element.Descendants()
                                                       .FirstOrDefault(
                                                           e => e.Name.LocalName.StartsWith("Practice"));
    }

    private static string GetSessionType(XElement sessionElement)
    {
        return sessionElement.Name.LocalName.StartsWith("Practice")
                   ? "Practice"
                   : sessionElement.Name.LocalName;
    }

    private static T? GetValue<T>(string? value, T defaultValue = default(T))
    {
        if(string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch(Exception exception)
        {
            throw new InvalidOperationException(
                $"Failed to convert value '{value}' to type {typeof(T).Name}.",
                exception);
        }
    }

    private static LmuSessionFile InitialiseResult(XElement element)
    {
        return new LmuSessionFile
        {
            Setting = element.Element("Setting")
                             ?.Value,
            ServerName = element.Element("ServerName")
                                ?.Value,
            ClientFuelVisible = GetValue<int>(element.Element("ClientFuelVisible")
                                                     ?.Value),
            PlayerFile = element.Element("PlayerFile")
                                ?.Value,
            DateTimeValue = GetValue<long>(element.Element("DateTime")
                                                  ?.Value),
            TimeString = element.Element("TimeString")
                                ?.Value,
            TrackVenue = element.Element("TrackVenue")
                                ?.Value,
            TrackCourse = element.Element("TrackCourse")
                                 ?.Value,
            TrackEvent = element.Element("TrackEvent")
                                ?.Value,
            TrackData = element.Element("TrackData")
                               ?.Value,
            TrackLength = GetValue<double>(element.Element("TrackLength")
                                                  ?.Value),
            GameVersion = element.Element("GameVersion")
                                 ?.Value,
            Dedicated = GetValue<int>(element.Element("Dedicated")
                                             ?.Value),
            RaceLaps = GetValue<int>(element.Element("RaceLaps")
                                            ?.Value),
            RaceTime = GetValue<int>(element.Element("RaceTime")
                                            ?.Value),
            MechanicalFailRate = GetValue<int>(element.Element("MechFailRate")
                                                      ?.Value,
                1),
            DamageMultiplier = GetValue<int>(element.Element("DamageMult")
                                                    ?.Value,
                100),
            FuelMultiplier = GetValue<int>(element.Element("FuelMult")
                                                  ?.Value,
                1),
            TireMultiplier = GetValue<int>(element.Element("TireMult")
                                                  ?.Value,
                1),
            VehiclesAllowed = element.Element("VehiclesAllowed")
                                     ?.Value,
            ParcFerme = GetValue<int>(element.Element("ParcFerme")
                                             ?.Value),
            FixedSetup = GetValue<int>(element.Element("FixedSetups")
                                              ?.Value),
            FreeSettings = GetValue<int>(element.Element("FreeSettings")
                                                ?.Value),

            FixedUpgrades = GetValue<int>(element.Element("FixedUpgrades")
                                                 ?.Value),
            LimitedTyres = GetValue<int>(element.Element("LimitedTyres")
                                                ?.Value),
            TireWarmers = GetValue<int>(element.Element("TireWarmers")
                                               ?.Value)
        };
    }
}