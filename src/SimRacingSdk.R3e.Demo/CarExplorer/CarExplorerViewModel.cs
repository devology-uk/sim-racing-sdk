using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Core.Models;

namespace SimRacingSdk.R3e.Demo.CarExplorer;

public partial class CarExplorerViewModel : ObservableObject
{
    private readonly IR3eCarInfoProvider carInfoProvider;

    [ObservableProperty]
    private bool isSelectableOnly = true;

    [ObservableProperty]
    private R3eCarInfo? selectedCar;

    [ObservableProperty]
    private string? selectedClass;

    public CarExplorerViewModel(IR3eCarInfoProvider carInfoProvider)
    {
        this.carInfoProvider = carInfoProvider;
    }

    public ObservableCollection<R3eCarInfo> Cars { get; } = [];
    public ObservableCollection<string> Classes { get; } = [];

    internal void Init()
    {
        this.RefreshClasses();
    }

    private bool IsShown(R3eCarInfo car)
    {
        return !this.IsSelectableOnly || !car.NotInGameSelector;
    }

    partial void OnIsSelectableOnlyChanged(bool value)
    {
        this.RefreshClasses();
    }

    partial void OnSelectedClassChanged(string? value)
    {
        this.Cars.Clear();

        IReadOnlyCollection<R3eCarInfo> carsInClass = value is null ? [] : this.carInfoProvider.GetCarInfosForClass(value);
        foreach(var car in carsInClass.Where(this.IsShown).OrderBy(car => car.Manufacturer).ThenBy(car => car.Name))
        {
            this.Cars.Add(car);
        }

        this.SelectedCar = this.Cars.FirstOrDefault();
    }

    // A class whose cars are all hidden drops out, e.g. Safety Cars while only selectable cars are shown.
    private void RefreshClasses()
    {
        var previousClass = this.SelectedClass;

        this.Classes.Clear();
        foreach(var carClass in this.carInfoProvider.GetCarClasses()
                                    .Where(carClass => this.carInfoProvider.GetCarInfosForClass(carClass).Any(this.IsShown)))
        {
            this.Classes.Add(carClass);
        }

        this.SelectedClass = this.Classes.Contains(previousClass ?? string.Empty) ? previousClass : this.Classes.FirstOrDefault();
    }
}
