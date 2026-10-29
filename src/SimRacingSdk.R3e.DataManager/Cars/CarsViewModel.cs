using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;
using SimRacingSdk.R3e.DataManager.GameData;

namespace SimRacingSdk.R3e.DataManager.Cars;

public partial class CarsViewModel : CatalogPageViewModel<CarInfo, CarEditorViewModel>
{
    private readonly ICatalogImporter<CarInfo> carImporter;

    [ObservableProperty]
    private string importStatusMessage = string.Empty;

    [ObservableProperty]
    private bool isSelectableOnly = true;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CopySpecCommand))]
    private CarSpecDonor? selectedSpecDonor;

    [ObservableProperty]
    private IReadOnlyList<CarSpecDonor> specDonors = [];

    [ObservableProperty]
    private CarFieldSuggestions suggestions = new([]);

    public CarsViewModel(
        IEntityRepository<CarInfo> carRepository,
        ICatalogGenerator<CarInfo> carProviderGenerator,
        ICatalogImporter<CarInfo> carImporter,
        ISessionStateStore sessionStateStore)
        : base("Cars", new CarEditorViewModel(), carRepository, carProviderGenerator, sessionStateStore)
    {
        this.carImporter = carImporter;
        this.Initialise();
    }

    protected override string DescribeGaps()
    {
        var selectableCars = this.Entities.Where(car => !car.NotInGameSelector).ToList();
        var missingSpecsCount = selectableCars.Count(car => car.PowerBhp is null || car.WeightWithDriverKg is null);
        var missingEngineCount = selectableCars.Count(car => string.IsNullOrWhiteSpace(car.EngineLayout)
                                                             || string.IsNullOrWhiteSpace(car.Drivetrain));
        return $" - of {selectableCars.Count} selectable cars, {missingSpecsCount} still missing power or weight, "
               + $"{missingEngineCount} engine or drive";
    }

    protected override string GroupOf(CarInfo entity)
    {
        return entity.Class;
    }

    protected override bool IsShown(CarInfo entity)
    {
        return !this.IsSelectableOnly || !entity.NotInGameSelector;
    }

    protected override void OnEntitiesChanged()
    {
        this.Suggestions = new CarFieldSuggestions(this.Entities);

        var selectedDonorId = this.SelectedSpecDonor?.Car.Id;
        this.SpecDonors = CarSpecDonor.From(this.Entities);
        this.SelectedSpecDonor = this.SpecDonors.FirstOrDefault(donor => donor.Car.Id == selectedDonorId);
    }

    protected override IEnumerable<CarInfo> Sort(IEnumerable<CarInfo> unsorted)
    {
        return unsorted.OrderBy(car => car.Manufacturer).ThenBy(car => car.Name).ThenBy(car => car.GameId);
    }

    protected override string? ValidationProblem(CarInfo entity)
    {
        return entity.GameId > 0 ? null : "it has no game id. Cars come from Import From Game";
    }

    partial void OnIsSelectableOnlyChanged(bool value)
    {
        this.RefreshShownEntities();
    }

    private bool CanCopySpec()
    {
        return this.SelectedSpecDonor is not null;
    }

    [RelayCommand(CanExecute = nameof(CanCopySpec))]
    private void CopySpec()
    {
        this.Editor.CopySpecFrom(this.SelectedSpecDonor!.Car);
    }

    [RelayCommand]
    private void ImportFromGame()
    {
        try
        {
            var result = this.carImporter.Import();
            this.Reload();
            this.ImportStatusMessage = result.Describe("cars");
        }
        catch(Exception exception) when(exception is IOException or JsonException or KeyNotFoundException)
        {
            this.ImportStatusMessage = $"Import failed - {exception.Message}";
        }
    }
}
