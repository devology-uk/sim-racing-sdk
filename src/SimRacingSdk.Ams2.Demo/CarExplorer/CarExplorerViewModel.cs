using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Core.Models;

namespace SimRacingSdk.Ams2.Demo.CarExplorer;

public partial class CarExplorerViewModel : ObservableObject
{
    private readonly IAms2CarInfoProvider carInfoProvider;

    [ObservableProperty]
    private string selectedClass = string.Empty;

    [ObservableProperty]
    private Ams2CarInfo? selectedCar = null;

    public ObservableCollection<string> CarClasses { get; } = [];
    public ObservableCollection<Ams2CarInfo> Cars { get; } = [];

    public CarExplorerViewModel(IAms2CarInfoProvider carInfoProvider)
    {
        this.carInfoProvider = carInfoProvider;
    }

    partial void OnSelectedClassChanged(string value)
    {
        this.Cars.Clear();

        foreach(var ams2CarInfo in this.carInfoProvider.GetCarInfosForClass(value))
        {
            this.Cars.Add(ams2CarInfo);
        }

        if(this.Cars.Count > 0)
        {
            this.SelectedCar = this.Cars[0];
        }
    }
    
    internal void Init()
    {
        foreach(var carClass in this.carInfoProvider.GetCarClasses())
        {
            this.CarClasses.Add(carClass);
        }

        if(this.CarClasses.Count > 0)
        {
            this.SelectedClass = this.CarClasses[0];
        }
    }
}