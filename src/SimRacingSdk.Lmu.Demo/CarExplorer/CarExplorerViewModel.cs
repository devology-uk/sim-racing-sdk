using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Demo.CarExplorer;

public partial class CarExplorerViewModel : ObservableObject
{
    private readonly ILmuCarInfoProvider carInfoProvider;

    [ObservableProperty]
    private LmuCarInfo? selectedCar = null;

    [ObservableProperty]
    private string selectedClass = string.Empty;

    public CarExplorerViewModel(ILmuCarInfoProvider carInfoProvider)
    {
        this.carInfoProvider = carInfoProvider;
    }

    public ObservableCollection<string> CarClasses { get; } = [];
    public ObservableCollection<LmuCarInfo> Cars { get; } = [];

    [RelayCommand]
    private void ExportCsv()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "lmu-cars.csv");

        using var streamWriter = new StreamWriter(filePath, options: new FileStreamOptions
        {
            Access = FileAccess.Write,
            Mode = FileMode.Create
        });

        foreach(var lmuCarInfo in this.carInfoProvider.GetCarInfos())
        {
            var carInfo = $"{lmuCarInfo.Class},{lmuCarInfo.DisplayName},{lmuCarInfo.DisplayName},2025,{lmuCarInfo.Manufacturer}";
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

        foreach(var lmuCarInfo in this.carInfoProvider.GetCarInfosForClass(value))
        {
            this.Cars.Add(lmuCarInfo);
        }

        if(this.Cars.Count > 0)
        {
            this.SelectedCar = this.Cars[0];
        }
    }
}