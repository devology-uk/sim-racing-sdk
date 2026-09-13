using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Demo.CarExplorer;

public partial class CarExplorerViewModel : ObservableObject
{
    private readonly IPmrCarInfoProvider carInfoProvider;

    [ObservableProperty]
    private PmrCarInfo? selectedCar;

    [ObservableProperty]
    private string selectedManufacturer = string.Empty;

    public CarExplorerViewModel(IPmrCarInfoProvider carInfoProvider)
    {
        this.carInfoProvider = carInfoProvider;
    }

    public ObservableCollection<PmrCarInfo> Cars { get; } = [];
    public ObservableCollection<string> Manufacturers { get; } = [];

    internal void Init()
    {
        this.Manufacturers.Clear();
        foreach(var manufacturer in this.carInfoProvider.GetManufacturers())
        {
            this.Manufacturers.Add(manufacturer);
        }

        if(this.Manufacturers.Count > 0)
        {
            this.SelectedManufacturer = this.Manufacturers[0];
        }
    }

    partial void OnSelectedManufacturerChanged(string value)
    {
        this.Cars.Clear();

        foreach(var pmrCarInfo in this.carInfoProvider.GetCarInfosForManufacturer(value))
        {
            this.Cars.Add(pmrCarInfo);
        }

        this.SelectedCar = this.Cars.Count > 0 ? this.Cars[0] : null;
    }
}
