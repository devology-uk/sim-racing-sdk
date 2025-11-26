using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Core.Models;

namespace SimRacingSdk.Ams2.Demo.CarExplorer;

public partial class CarExplorerViewModel : ObservableObject
{
    private readonly IAms2CarInfoProvider carInfoProvider;

    [ObservableProperty]
    private Ams2CarInfo? selectedCar = null;

    [ObservableProperty]
    private string selectedClass = string.Empty;

    public CarExplorerViewModel(IAms2CarInfoProvider carInfoProvider)
    {
        this.carInfoProvider = carInfoProvider;
    }

    public ObservableCollection<string> CarClasses { get; } = [];
    public ObservableCollection<Ams2CarInfo> Cars { get; } = [];

    [RelayCommand]
    private void ExportCsv()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ams2-cars.csv");

        using var streamWriter = new StreamWriter(filePath, options: new FileStreamOptions
        {
            Access = FileAccess.Write,
            Mode = FileMode.Create
        });

        foreach(var ams2CarInfo in this.carInfoProvider.GetCarInfos())
        {
            var carInfo = $"{ams2CarInfo.Class},{ams2CarInfo.Model},{ams2CarInfo.Model},2025,{ams2CarInfo.Manufacturer}";
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