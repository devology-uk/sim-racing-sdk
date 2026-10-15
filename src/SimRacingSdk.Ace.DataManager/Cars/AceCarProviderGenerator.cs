using System.IO;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Generation;
using SimRacingSdk.DataManager.Shared.Storage;
using static SimRacingSdk.DataManager.Shared.Generation.CSharpLiteral;

namespace SimRacingSdk.Ace.DataManager.Cars;

// Regenerates SimRacingSdk.Ace.Core's whole car-catalog surface - the car model, its setup schema,
// the interface and the provider - from the cars captured here.
public class AceCarProviderGenerator : ICatalogGenerator<CarInfo>
{
    private readonly IDataPathProvider dataPathProvider;
    private readonly DataManagerOptions options;

    public AceCarProviderGenerator(IDataPathProvider dataPathProvider, DataManagerOptions options)
    {
        this.dataPathProvider = dataPathProvider;
        this.options = options;
    }

    public string Generate(IReadOnlyCollection<CarInfo> entities)
    {
        var coreRoot = this.dataPathProvider.GetProjectFolder(this.options.CoreProjectName);

        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "AceCarInfo.cs"), BuildModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "AceSetupSchema.cs"), BuildSetupSchemaSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Abstractions", "IAceCarInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "AceCarInfoProvider.cs");
        GeneratedSource.Write(providerPath, BuildProviderSource(entities));
        return providerPath;
    }

    private static string BuildEntryLine(CarInfo car)
    {
        return "        new() { "
               + $"Body = {Text(car.Body)}, "
               + $"BatteryKwh = {NullableNumber(car.BatteryKwh)}, "
               + $"Categories = {Text(car.Categories)}, "
               + $"Cylinders = {NullableNumber(car.Cylinders)}, "
               + $"DisplacementL = {NullableNumber(car.DisplacementL)}, "
               + $"DriveLayout = {Text(car.DriveLayout)}, "
               + $"Engine = {Text(car.Engine)}, "
               + $"EngineLayout = {Text(car.EngineLayout)}, "
               + $"FuelTankL = {NullableNumber(car.FuelTankL)}, "
               + $"Gears = {NullableNumber(car.Gears)}, "
               + $"Manufacturer = {Text(car.Manufacturer)}, "
               + $"MaxRpm = {NullableNumber(car.MaxRpm)}, "
               + $"MaxSpeedKmh = {car.MaxSpeedKmh}, "
               + $"ModelId = {Text(car.ModelId)}, "
               + $"Name = {Text(car.Name)}, "
               + $"PerformanceRating = {Number(car.PerformanceRating)}, "
               + $"PowerPs = {car.PowerPs}, "
               + $"RacingClass = {Text(car.RacingClass)}, "
               + $"SetupSchema = {BuildSetupSchemaEntry(car.SetupSchema)}, "
               + $"TorqueNm = {car.TorqueNm}, "
               + $"TyreCompounds = {TextArray(car.TyreCompounds)}, "
               + $"Variant = {Text(car.Variant)}, "
               + $"WeightKg = {car.WeightKg}, "
               + $"Year = {car.Year}, "
               + $"ZeroToOneHundredKmh = {Number(car.ZeroToOneHundredKmh)} "
               + "},";
    }

    // Enabled is always written; other flags only when true, since false is the default.
    private static string BuildSetupSchemaEntry(SetupSchema schema)
    {
        var tyres = Initialiser(
            $"Enabled = {Bool(schema.Tyres.Enabled)}",
            $"Front = {BuildAxleEntry(schema.Tyres.Front)}",
            $"Rear = {BuildAxleEntry(schema.Tyres.Rear)}");

        var electronics = Section(schema.Electronics.Enabled,
            ("HasTractionControl", schema.Electronics.HasTractionControl),
            ("HasTractionControl2", schema.Electronics.HasTractionControl2),
            ("HasAbs", schema.Electronics.HasAbs),
            ("HasEsc", schema.Electronics.HasEsc),
            ("HasTurboBoost", schema.Electronics.HasTurboBoost),
            ("HasEngineMap", schema.Electronics.HasEngineMap),
            ("HasErsDeploymentMap", schema.Electronics.HasErsDeploymentMap),
            ("HasErsRechargeLevel", schema.Electronics.HasErsRechargeLevel),
            ("HasErsHeatCharging", schema.Electronics.HasErsHeatCharging),
            ("HasTelemetryLaps", schema.Electronics.HasTelemetryLaps));

        var fuelAssignments = new List<string> { $"Enabled = {Bool(schema.FuelAndStrategy.Enabled)}" };
        if(schema.FuelAndStrategy.HasFuel)
        {
            fuelAssignments.Add("HasFuel = true");
        }

        if(schema.FuelAndStrategy.TyreCompoundMode != TyreCompoundMode.None)
        {
            fuelAssignments.Add($"TyreCompoundMode = AceTyreCompoundMode.{schema.FuelAndStrategy.TyreCompoundMode}");
        }

        var suspension = Section(schema.Suspension.Enabled,
            ("HasWheelRate", schema.Suspension.HasWheelRate),
            ("HasFrontAntiRollBar", schema.Suspension.HasFrontAntiRollBar),
            ("HasRearAntiRollBar", schema.Suspension.HasRearAntiRollBar),
            ("HasBrakeBias", schema.Suspension.HasBrakeBias),
            ("HasBrakeTorqueMultiplier", schema.Suspension.HasBrakeTorqueMultiplier),
            ("HasSteerRatio", schema.Suspension.HasSteerRatio),
            ("HasBumpstopRate", schema.Suspension.HasBumpstopRate),
            ("HasBumpstopRange", schema.Suspension.HasBumpstopRange),
            ("HasDifferentialPreload", schema.Suspension.HasDifferentialPreload),
            ("HasDifferentialCoast", schema.Suspension.HasDifferentialCoast),
            ("HasDifferentialPower", schema.Suspension.HasDifferentialPower));

        var dampers = Section(schema.Dampers.Enabled,
            ("HasFastBump", schema.Dampers.HasFastBump),
            ("HasFastRebound", schema.Dampers.HasFastRebound));

        var aero = Section(schema.Aero.Enabled,
            ("HasFrontRideHeight", schema.Aero.HasFrontRideHeight),
            ("HasRearRideHeight", schema.Aero.HasRearRideHeight),
            ("HasFrontWingAngle", schema.Aero.HasFrontWingAngle),
            ("HasRearWingAngle", schema.Aero.HasRearWingAngle));

        return Initialiser(
            $"Tyres = {tyres}",
            $"Electronics = {electronics}",
            $"FuelAndStrategy = {Initialiser(fuelAssignments.ToArray())}",
            $"Suspension = {suspension}",
            $"Dampers = {dampers}",
            $"Aero = {aero}");
    }

    private static string BuildAxleEntry(TyreAxleSchema axle)
    {
        var flags = new[]
            {
                ("HasPressure", axle.HasPressure),
                ("HasCamber", axle.HasCamber),
                ("HasToe", axle.HasToe),
                ("HasCaster", axle.HasCaster)
            }
            .Where(flag => flag.Item2)
            .Select(flag => $"{flag.Item1} = true")
            .ToArray();
        return flags.Length == 0 ? "new()" : Initialiser(flags);
    }

    private static string Section(bool enabled, params (string Name, bool Value)[] flags)
    {
        var assignments = flags.Where(flag => flag.Value).Select(flag => $"{flag.Name} = true").Prepend($"Enabled = {Bool(enabled)}");
        return Initialiser(assignments.ToArray());
    }

    private static string Initialiser(params string[] assignments)
    {
        return $"new() {{ {string.Join(", ", assignments)} }}";
    }

    private static string BuildInterfaceSource()
    {
        return """
               using SimRacingSdk.Ace.Core.Models;

               namespace SimRacingSdk.Ace.Core.Abstractions;

               public interface IAceCarInfoProvider
               {
                   // ModelId is the preset id string from the ACE Dedicated Server's cars.json (e.g.
                   // "preset_m4gt3_mech_1") - see AceCarInfoProvider.cs.
                   AceCarInfo? FindByModelId(string modelId);
                   AceCarInfo? FindByName(string name);

                   // Matches against AceCarInfo.AceName, the Manufacturer+Name form Ace's shared memory
                   // actually reports as CarModel (e.g. "BMW M4 GT3 Evo") - use this, not FindByName, when
                   // matching a shared-memory CarModel string.
                   AceCarInfo? FindByAceName(string aceName);
                   IReadOnlyCollection<AceCarInfo> GetCarInfos();
                   IReadOnlyCollection<AceCarInfo> GetCarInfosForManufacturer(string manufacturer);
                   IReadOnlyCollection<string> GetManufacturers();
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Ace.Core.Models;

               public record AceCarInfo
               {
                   public string Body { get; init; }
                   public double? BatteryKwh { get; init; }
                   public string Categories { get; init; }
                   public int? Cylinders { get; init; }
                   public double? DisplacementL { get; init; }
                   public string DriveLayout { get; init; }
                   public string Engine { get; init; }
                   public string EngineLayout { get; init; }
                   public double? FuelTankL { get; init; }
                   public int? Gears { get; init; }
                   public string Manufacturer { get; init; }
                   public int? MaxRpm { get; init; }
                   public int MaxSpeedKmh { get; init; }
                   public string ModelId { get; init; }
                   public string Name { get; init; }
                   public double PerformanceRating { get; init; }
                   public int PowerPs { get; init; }
                   public string RacingClass { get; init; }
                   public AceSetupSchema SetupSchema { get; init; } = new();
                   public int TorqueNm { get; init; }
                   public string[] TyreCompounds { get; init; } = [];
                   public string Variant { get; init; }
                   public int WeightKg { get; init; }
                   public int Year { get; init; }
                   public double ZeroToOneHundredKmh { get; init; }

                   public string DisplayName =>
                       string.IsNullOrEmpty(this.Variant)
                           ? $"{this.Manufacturer} {this.Name}"
                           : $"{this.Manufacturer} {this.Name} - {this.Variant}";

                   // Matches the CarModel string Ace's shared memory actually reports (e.g. "BMW M4 GT3 Evo") -
                   // unlike DisplayName, never includes Variant, since shared memory has no variant concept.
                   public string AceName => $"{this.Manufacturer} {this.Name}";
               }

               """;
    }

    private static string BuildSetupSchemaSource()
    {
        return """
               namespace SimRacingSdk.Ace.Core.Models;

               public record AceSetupSchema
               {
                   public AceTyresSchema Tyres { get; init; } = new();
                   public AceElectronicsSchema Electronics { get; init; } = new();
                   public AceFuelAndStrategySchema FuelAndStrategy { get; init; } = new();
                   public AceSuspensionSchema Suspension { get; init; } = new();
                   public AceDampersSchema Dampers { get; init; } = new();
                   public AceAeroSchema Aero { get; init; } = new();
               }

               public record AceTyresSchema
               {
                   public bool Enabled { get; init; }
                   public AceTyreAxleSchema Front { get; init; } = new();
                   public AceTyreAxleSchema Rear { get; init; } = new();
               }

               public record AceTyreAxleSchema
               {
                   public bool HasPressure { get; init; }
                   public bool HasCamber { get; init; }
                   public bool HasToe { get; init; }
                   public bool HasCaster { get; init; }
               }

               public record AceElectronicsSchema
               {
                   public bool Enabled { get; init; }
                   public bool HasTractionControl { get; init; }
                   public bool HasTractionControl2 { get; init; }
                   public bool HasAbs { get; init; }
                   public bool HasEsc { get; init; }
                   public bool HasTurboBoost { get; init; }
                   public bool HasEngineMap { get; init; }
                   public bool HasErsDeploymentMap { get; init; }
                   public bool HasErsRechargeLevel { get; init; }
                   public bool HasErsHeatCharging { get; init; }
                   public bool HasTelemetryLaps { get; init; }
               }

               public record AceFuelAndStrategySchema
               {
                   public bool Enabled { get; init; }
                   public bool HasFuel { get; init; }
                   public AceTyreCompoundMode TyreCompoundMode { get; init; } = AceTyreCompoundMode.None;
               }

               // None: no compound control at all. Unified: one control sets all four corners together (Audi).
               // FrontOnly: one control, front axle only, no rear control (some road-derived cars). FrontAndRear:
               // independent front and rear controls (most racing-class cars).
               public enum AceTyreCompoundMode
               {
                   None,
                   Unified,
                   FrontOnly,
                   FrontAndRear
               }

               public record AceSuspensionSchema
               {
                   public bool Enabled { get; init; }
                   public bool HasWheelRate { get; init; }
                   public bool HasFrontAntiRollBar { get; init; }
                   public bool HasRearAntiRollBar { get; init; }
                   public bool HasBrakeBias { get; init; }
                   public bool HasBrakeTorqueMultiplier { get; init; }
                   public bool HasSteerRatio { get; init; }
                   public bool HasBumpstopRate { get; init; }
                   public bool HasBumpstopRange { get; init; }
                   public bool HasDifferentialPreload { get; init; }
                   public bool HasDifferentialCoast { get; init; }
                   public bool HasDifferentialPower { get; init; }
               }

               public record AceDampersSchema
               {
                   public bool Enabled { get; init; }
                   public bool HasFastBump { get; init; }
                   public bool HasFastRebound { get; init; }
               }

               public record AceAeroSchema
               {
                   public bool Enabled { get; init; }
                   public bool HasFrontRideHeight { get; init; }
                   public bool HasRearRideHeight { get; init; }
                   public bool HasFrontWingAngle { get; init; }
                   public bool HasRearWingAngle { get; init; }
               }

               """;
    }

    // Manufacturer + Name (shared memory's AceName) and ModelId are each shared by several cars, and the
    // Find methods return the first match - ModelId then PerformanceRating within a name keeps the order
    // the catalog has always had (the base variant first).
    private static string BuildProviderSource(IEnumerable<CarInfo> cars)
    {
        var entryLines = cars.OrderBy(car => car.Manufacturer, StringComparer.Ordinal)
            .ThenBy(car => car.Name, StringComparer.Ordinal)
            .ThenBy(car => car.ModelId, StringComparer.Ordinal)
            .ThenBy(car => car.PerformanceRating)
            .Select(BuildEntryLine);

        return $$"""
                 using SimRacingSdk.Ace.Core.Abstractions;
                 using SimRacingSdk.Ace.Core.Models;

                 namespace SimRacingSdk.Ace.Core;

                 public class AceCarInfoProvider : IAceCarInfoProvider
                 {
                     private const int MaxReportedCarModelLength = 32;

                     private static AceCarInfoProvider? singletonInstance;

                     private readonly List<AceCarInfo> cars =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     public static AceCarInfoProvider Instance => singletonInstance ??= new AceCarInfoProvider();

                     public AceCarInfo? FindByModelId(string modelId)
                     {
                         return this.cars.FirstOrDefault(c => c.ModelId == modelId);
                     }

                     public AceCarInfo? FindByName(string name)
                     {
                         return this.cars.FirstOrDefault(c => c.Name == name);
                     }

                     // Shared memory holds CarModel in a 33-character buffer, so a longer name arrives cut off at 32
                     // (e.g. "BMW M3 E30 Sport Evo (Evolution ") - at that length only the start can be matched.
                     public AceCarInfo? FindByAceName(string aceName)
                     {
                         var exactMatch = this.cars.FirstOrDefault(
                             c => string.Equals(c.AceName, aceName, StringComparison.OrdinalIgnoreCase));
                         if(exactMatch != null || aceName.Length < MaxReportedCarModelLength)
                         {
                             return exactMatch;
                         }

                         return this.cars.FirstOrDefault(c => c.AceName.StartsWith(aceName, StringComparison.OrdinalIgnoreCase));
                     }

                     public IReadOnlyCollection<AceCarInfo> GetCarInfos()
                     {
                         return this.cars.AsReadOnly();
                     }

                     public IReadOnlyCollection<AceCarInfo> GetCarInfosForManufacturer(string manufacturer)
                     {
                         return this.cars.Where(c => c.Manufacturer == manufacturer)
                                          .ToList()
                                          .AsReadOnly();
                     }

                     public IReadOnlyCollection<string> GetManufacturers()
                     {
                         return this.cars.Select(c => c.Manufacturer)
                                          .Distinct()
                                          .OrderBy(m => m)
                                          .ToList()
                                          .AsReadOnly();
                     }
                 }

                 """;
    }
}
