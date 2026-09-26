using SimRacingSdk.Pmr.Setups.Abstractions;
using SimRacingSdk.Pmr.Setups.Models.Files;
using SimRacingSdk.Pmr.Setups.Models.Maps;
using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Services;

public class PmrSetupPresenter : IPmrSetupPresenter
{
    // In the game's own tab order.
    private static readonly (string Name, Func<PmrSetupMap, IReadOnlyList<PmrSetupFieldMap>> Fields)[] TabLayout =
    [
        ("Tyres & Chassis", map => map.TyresAndChassis),
        ("Suspension", map => map.Suspension),
        ("Engine & Drivetrain", map => map.EngineAndDrivetrain),
        ("Steering Wheel", map => map.SteeringWheel)
    ];

    private static PmrSetupPresenter? singletonInstance;

    private readonly IPmrSetupMapProvider setupMapProvider;

    public PmrSetupPresenter(IPmrSetupMapProvider setupMapProvider)
    {
        this.setupMapProvider = setupMapProvider;
    }

    public static PmrSetupPresenter Instance => singletonInstance ??= new PmrSetupPresenter(PmrSetupMapProvider.Instance);

    public PmrSetupComparison Compare(PmrSetupFile first, PmrSetupFile second, PmrUnitSystem unitSystem)
    {
        if(!IsSameCar(first.Name, second.Name))
        {
            throw new ArgumentException("Only setups for the same car can be compared.", nameof(second));
        }

        var firstView = this.Present(first, unitSystem);
        var secondView = this.Present(second, unitSystem);

        return new PmrSetupComparison
        {
            Differences = PmrSetupDifferenceFinder.Between(firstView, secondView),
            First = firstView,
            Second = secondView
        };
    }

    public PmrSetupView Present(PmrSetupFile setup, PmrUnitSystem unitSystem)
    {
        var map = this.FindMap(setup.Name);
        return map is null ? WithoutMap(setup, unitSystem) : WithMap(setup, map, unitSystem);
    }

    private static bool IsSameCar(PmrSetupFileName first, PmrSetupFileName second)
    {
        return string.Equals(first.VehicleGameId, second.VehicleGameId, StringComparison.OrdinalIgnoreCase);
    }

    private static PmrSetupFieldView PresentField(IGrouping<string, PmrSetupFieldMap> rows, PmrSetupFile setup, PmrUnitSystem unitSystem)
    {
        return new PmrSetupFieldView
        {
            Name = rows.Key,
            Values = rows.SelectMany(row => PmrSetupPositions.For(row.Scope)
                                                             .Select(position => PmrSetupValueReader.Read(row, position, setup, unitSystem)))
                         .ToList()
        };
    }

    // A field whose front and rear ranges differ is two map rows with the same name; they're shown
    // as one field, as on the game's screen.
    private static PmrSetupSectionView PresentSection(IGrouping<string, PmrSetupFieldMap> section, PmrSetupFile setup, PmrUnitSystem unitSystem)
    {
        return new PmrSetupSectionView
        {
            Fields = section.GroupBy(row => row.Name)
                            .Select(rows => PresentField(rows, setup, unitSystem))
                            .ToList(),
            Name = section.Key
        };
    }

    private static PmrSetupTabView PresentTab(string name, IReadOnlyList<PmrSetupFieldMap> fields, PmrSetupFile setup, PmrUnitSystem unitSystem)
    {
        return new PmrSetupTabView
        {
            Name = name,
            Sections = fields.GroupBy(field => field.Section)
                             .Select(section => PresentSection(section, setup, unitSystem))
                             .ToList()
        };
    }

    private static PmrSetupView WithMap(PmrSetupFile setup, PmrSetupMap map, PmrUnitSystem unitSystem)
    {
        var tabs = TabLayout.Select(tab => PresentTab(tab.Name, tab.Fields(map), setup, unitSystem))
                            .Where(tab => tab.Sections.Count > 0)
                            .ToList();

        return new PmrSetupView
        {
            FileName = setup.Name,
            HasMap = true,
            IsFullyMapped = tabs.SelectMany(tab => tab.Sections)
                                .SelectMany(section => section.Fields)
                                .SelectMany(field => field.Values)
                                .All(value => value.Status != PmrSetupValueStatus.NotMapped),
            Tabs = tabs,
            UnitSystem = unitSystem
        };
    }

    private static PmrSetupView WithoutMap(PmrSetupFile setup, PmrUnitSystem unitSystem)
    {
        return new PmrSetupView
        {
            FileName = setup.Name,
            HasMap = false,
            IsFullyMapped = false,
            MapExplanation = PmrSetupExplanations.NoMap,
            Tabs = [],
            UnitSystem = unitSystem
        };
    }

    private PmrSetupMap? FindMap(PmrSetupFileName fileName)
    {
        var mapByCar = fileName.Car is null ? null : this.setupMapProvider.FindByCarId(fileName.Car.Id);
        return mapByCar ?? this.setupMapProvider.FindByVehicleGameId(fileName.VehicleGameId);
    }
}
