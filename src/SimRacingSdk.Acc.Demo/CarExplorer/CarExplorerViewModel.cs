using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Demo.CarExplorer;

public partial class CarExplorerViewModel : ObservableObject
{
    private readonly IAccCarInfoProvider carInfoProvider;

    [ObservableProperty]
    private AccCarInfo? selectedCar = null;

    [ObservableProperty]
    private string selectedClass = string.Empty;

    public CarExplorerViewModel(IAccCarInfoProvider carInfoProvider)
    {
        this.carInfoProvider = carInfoProvider;
    }

    public ObservableCollection<string> CarClasses { get; } = [];
    public ObservableCollection<AccCarInfo> Cars { get; } = [];

    [RelayCommand]
    private void ExportCsv()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "acc-cars.csv");

        using var streamWriter = new StreamWriter(filePath,
            options: new FileStreamOptions
            {
                Access = FileAccess.Write,
                Mode = FileMode.Create
            });

        foreach(var accCarInfo in this.carInfoProvider.GetCarInfos())
        {
            var carInfo =
                $"{accCarInfo.Class},{accCarInfo.AccName},{accCarInfo.DisplayName},{accCarInfo.Year},{accCarInfo.ManufacturerTag}";
            streamWriter.WriteLine(carInfo);
            streamWriter.Flush();
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
}