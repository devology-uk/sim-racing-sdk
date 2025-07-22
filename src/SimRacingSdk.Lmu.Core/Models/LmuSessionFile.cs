using System.Diagnostics;
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
                Upload = GetInt(connectionTypeElement.Attribute("upload")
                                                            ?.Value),
                Download = GetInt(connectionTypeElement.Attribute("download")
                                                              ?.Value)
            };
        }
    }

    private static void AddDriverControlAndAids(XElement driverElement, LmuDriver driver)
    {
        var controlAndAidsElement = driverElement.Element("ControlAndAids");
        if(controlAndAidsElement != null)
        {
            driver.ControlAndAids = new LmuControlAndAids(GetInt(controlAndAidsElement
                                                                        .Attribute("startLap")
                                                                        ?.Value),
                GetInt(controlAndAidsElement.Attribute("endLap")
                                                   ?.Value),
                controlAndAidsElement.Value);
        }
    }

    private static void AddDriverLaps(XElement driverElement, LmuDriver driver)
    {
        var lapElements = driverElement.Elements("Lap").Where(l => !l.Value.Contains("-"));
        foreach(var lapElement in lapElements)
        {
            var lap = new LmuDriverLap
            {
                Number = GetInt(lapElement.Attribute("num")
                                                 ?.Value),
                EventTiming = GetDouble(lapElement.Attribute("et")
                                                         ?.Value),
                Position = GetInt(lapElement.Attribute("p")
                                                   ?.Value),
                Sector1Time = GetDouble(lapElement.Attribute("s1")
                                                         ?.Value),
                Sector2Time = GetDouble(lapElement.Attribute("s2")
                                                         ?.Value),
                Sector3Time = GetDouble(lapElement.Attribute("s3")
                                                         ?.Value),
                TopSpeed = GetDouble(lapElement.Attribute("topspeed")
                                                      ?.Value),
                Fuel = GetDouble(lapElement.Attribute("fuel")
                                                  ?.Value),
                FuelUsed = GetDouble(lapElement.Attribute("fuelUsed")
                                                      ?.Value),
                VirtualEnergy = GetDouble(lapElement.Attribute("ve")
                                                           ?.Value),
                VirtualEnergyUsed = GetDouble(lapElement.Attribute("veUsed")
                                                               ?.Value),
                TyreWearFrontLeft = GetDouble(lapElement.Attribute("twfl")
                                                               ?.Value),
                TyreWearFrontRight = GetDouble(lapElement.Attribute("twfr")
                                                                ?.Value),
                TyreWearRearLeft = GetDouble(lapElement.Attribute("twrl")
                                                              ?.Value),
                TyreWearRearRight = GetDouble(lapElement.Attribute("twrr")
                                                               ?.Value),
                TyreFrontCompound = lapElement.Attribute("fcompound")
                                              ?.Value,
                TyreRearCompound = lapElement.Attribute("rcompound")
                                             ?.Value,
                TyreFrontLeftCompound = lapElement.Attribute("FL")
                                                  ?.Value,
                TyreFrontRightCompound = lapElement.Attribute("FR")
                                                   ?.Value,
                TyreRearLeftCompound = lapElement.Attribute("RL")
                                                 ?.Value,
                TyreRearRightCompound = lapElement.Attribute("RR")
                                                  ?.Value,
                LapTime = GetDouble(lapElement.Value)
            };
            driver.Laps.Add(lap);
        }
    }

    private static void AddDrivers(XElement element, LmuDrivingSession session)
    {
        var driverElements = element.Elements("Driver");
        foreach(var driverElement in driverElements)
        {
            var driver = new LmuDriver
            {
                Name = driverElement.Element("Name")
                                    ?.Value,
                Connected = GetInt(driverElement.Element("Connected")
                                                       ?.Value),
                VehicleFile = driverElement.Element("VehicleFile")
                                           ?.Value,
                UpgradeCode = driverElement.Element("UpgradeCode")
                                           ?.Value,
                VehicleName = driverElement.Element("VehName")
                                           ?.Value,
                Category = driverElement.Element("Category")
                                        ?.Value,
                CarType = driverElement.Element("CarType")
                                       ?.Value,
                CarClass = driverElement.Element("CarClass")
                                        ?.Value,
                CarNumber = GetInt(driverElement.Element("CarNumber")
                                                       ?.Value),
                TeamName = driverElement.Element("TeamName")
                                        ?.Value,
                IsPlayer = GetInt(driverElement.Element("isPlayer")
                                                      ?.Value),
                ServerScored = GetInt(driverElement.Element("ServerScored")
                                                          ?.Value),
                GridPosition = GetInt(driverElement.Element("GridPos")
                                                          ?.Value),
                Position = GetInt(driverElement.Element("Position")
                                                      ?.Value),
                ClassGridPosition = GetInt(driverElement.Element("ClassGridPos")
                                                               ?.Value),
                ClassPosition = GetInt(driverElement.Element("ClassPosition")
                                                           ?.Value),
                LapRankIncludingDiscos = GetInt(driverElement.Element("LapRankIncludingDiscos")
                                                                    ?.Value),
                BestLap = GetDouble(driverElement.Element("BestLapTime")
                                                        ?.Value),
                FinishTime = GetDouble(driverElement.Element("FinishTime")
                                                           ?.Value),
                LapCount = GetInt(driverElement.Element("Laps")
                                                      ?.Value),
                PitStops = GetInt(driverElement.Element("PitStops")
                                                      ?.Value),
                FinishStatus = driverElement.Element("FinishStatus")
                                            ?.Value
            };

            AddDriverControlAndAids(driverElement, driver);
            AddDriverLaps(driverElement, driver);
            if(driver.Laps.Any())
            {
                driver.BestSector1 = driver.Laps.Min(l => l.Sector1Time);
                driver.BestSector2 = driver.Laps.Min(l => l.Sector2Time);
                driver.BestSector3 = driver.Laps.Min(l => l.Sector3Time);
            }

            session.Drivers.Add(driver);
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
                    session.Stream.Add(new LmuIncidentStreamEvent(GetDouble(eventElement
                            .Attribute("et")
                            ?.Value),
                        eventElement.Value));
                    break;
                case "Score":
                    session.Stream.Add(new LmuScoreStreamEvent(GetDouble(eventElement.Attribute("et")
                            ?.Value),
                        eventElement.Value));
                    break;
                case "Sector":
                    session.Stream.Add(new LmuSectorStreamEvent(GetDouble(eventElement.Attribute("et")
                            ?.Value),
                        eventElement.Value,
                        eventElement.Element("Driver")
                                    ?.Value ?? string.Empty,
                        GetInt(eventElement.Element("ID")
                                                  ?.Value),
                        GetInt(eventElement.Element("Sector")
                                                  ?.Value),
                        eventElement.Element("Class")
                                    ?.Value ?? string.Empty));
                    break;
                case "Sent":
                    session.Stream.Add(new LmuSentStreamEvent(GetDouble(eventElement.Attribute("et")
                            ?.Value),
                        eventElement.Value));
                    break;
                case "TrackLimits":
                    session.Stream.Add(new LmuTrackLimitsStreamEvent(GetDouble(eventElement
                            .Attribute("et")
                            ?.Value),
                        eventElement.Value,
                        eventElement.Element("Driver")
                                    ?.Value ?? string.Empty,
                        GetInt(eventElement.Element("ID")
                                                  ?.Value),
                        GetInt(eventElement.Element("Lap")
                                                  ?.Value),
                        GetInt(eventElement.Element("WarningPoints")
                                                  ?.Value),
                        GetInt(eventElement.Element("CurrentPoints")
                                                  ?.Value),
                        GetInt(eventElement.Element("Resolution")
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
            DateTimeValue = GetInt(sessionElement.Element("DateTime")
                                                        ?.Value),
            TimeString = sessionElement.Element("TimeString")
                                       ?.Value,
            Laps = GetInt(sessionElement.Element("Laps")
                                               ?.Value),
            Minutes = GetInt(sessionElement.Element("Minutes")
                                                  ?.Value),
            FormationAndStart = GetInt(sessionElement.Element("FormationAndStart")
                                                            ?.Value),
            MostLapsCompleted = GetInt(sessionElement.Element("MostLapsCompleted")
                                                            ?.Value)
        };

        AddEventStream(sessionElement, session);
        AddDrivers(sessionElement, session);

        result.Session = session;
    }

    private static double GetDouble(string? value, double defaultValue = 0.0)
    {
        if(string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        if(double.TryParse(value, out var result))
        {
            return result;
        }

        Debug.WriteLine($"Failed to parse value '{value}' as double.");
        return defaultValue;
    }

    private static int GetInt(string? value, int defaultValue = 0)
    {
        if(string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        if(int.TryParse(value, out var result))
        {
            return result;
        }

        Debug.WriteLine($"Failed to parse value '{value}' as int.");
        return defaultValue;
    }

    private static long GetLong(string? value, long defaultValue = 0)
    {
        if(string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        if(long.TryParse(value, out var result))
        {
            return result;
        }

        Debug.WriteLine($"Failed to parse value '{value}' as long.");
        return defaultValue;
    }

    private static XElement? GetSessionElement(XElement element)
    {
        var sessionElement = element.Element("Race");
        if(sessionElement != null)
        {
            return sessionElement;
        }

        return element.Element("Qualify") ?? element.Descendants()
                                                       .FirstOrDefault(
                                                           e => e.Name.LocalName.StartsWith("Practice"));
    }

    private static string GetSessionType(XElement sessionElement)
    {
        return sessionElement.Name.LocalName.StartsWith("Practice")
                   ? "Practice"
                   : sessionElement.Name.LocalName;
    }

    private static LmuSessionFile InitialiseResult(XElement element)
    {
        return new LmuSessionFile
        {
            Setting = element.Element("Setting")
                             ?.Value,
            ServerName = element.Element("ServerName")
                                ?.Value,
            ClientFuelVisible = GetInt(element.Element("ClientFuelVisible")
                                                     ?.Value),
            PlayerFile = element.Element("PlayerFile")
                                ?.Value,
            DateTimeValue = GetLong(element.Element("DateTime")
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
            TrackLength = GetDouble(element.Element("TrackLength")
                                                  ?.Value),
            GameVersion = element.Element("GameVersion")
                                 ?.Value,
            Dedicated = GetInt(element.Element("Dedicated")
                                             ?.Value),
            RaceLaps = GetInt(element.Element("RaceLaps")
                                            ?.Value),
            RaceTime = GetInt(element.Element("RaceTime")
                                            ?.Value),
            MechanicalFailRate = GetInt(element.Element("MechFailRate")
                                                      ?.Value,
                1),
            DamageMultiplier = GetInt(element.Element("DamageMult")
                                                    ?.Value,
                100),
            FuelMultiplier = GetInt(element.Element("FuelMult")
                                                  ?.Value,
                1),
            TireMultiplier = GetInt(element.Element("TireMult")
                                                  ?.Value,
                1),
            VehiclesAllowed = element.Element("VehiclesAllowed")
                                     ?.Value,
            ParcFerme = GetInt(element.Element("ParcFerme")
                                             ?.Value),
            FixedSetup = GetInt(element.Element("FixedSetups")
                                              ?.Value),
            FreeSettings = GetInt(element.Element("FreeSettings")
                                                ?.Value),

            FixedUpgrades = GetInt(element.Element("FixedUpgrades")
                                                 ?.Value),
            LimitedTyres = GetInt(element.Element("LimitedTyres")
                                                ?.Value),
            TireWarmers = GetInt(element.Element("TireWarmers")
                                               ?.Value)
        };
    }
}