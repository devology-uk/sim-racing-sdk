using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Pmr.DataManager.Cars;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public partial class SetupMapsViewModel : ObservableObject
{
    private readonly ISetupMapRepository setupMapRepository;

    [ObservableProperty]
    private CarInfo? selectedCar;

    public SetupMapsViewModel(ICarRepository carRepository, ISetupMapRepository setupMapRepository)
    {
        this.setupMapRepository = setupMapRepository;

        foreach(var car in carRepository.GetAll().OrderBy(car => car.Manufacturer).ThenBy(car => car.Name))
        {
            this.Cars.Add(car);
        }

        this.SelectedCar = this.Cars.FirstOrDefault();
    }

    public ObservableCollection<CarInfo> Cars { get; } = [];
    public SetupMapEditorViewModel Editor { get; } = new();

    [RelayCommand]
    private void SaveSetupMap()
    {
        if(this.SelectedCar is null)
        {
            return;
        }

        this.setupMapRepository.Save(this.Editor.ToSetupMap());
    }

    partial void OnSelectedCarChanged(CarInfo? value)
    {
        this.Editor.LoadFrom(value?.Id ?? string.Empty, value is null ? null : this.setupMapRepository.FindByCarId(value.Id));
    }
}
