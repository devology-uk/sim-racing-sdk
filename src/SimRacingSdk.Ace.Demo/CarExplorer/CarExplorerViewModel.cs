using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Demo.CarExplorer;

public partial class CarExplorerViewModel : ObservableObject
{
    private readonly IAceCarInfoProvider carInfoProvider;

    [ObservableProperty]
    private AceCarInfo? selectedCar = null;

    [ObservableProperty]
    private string selectedManufacturer = string.Empty;

    public CarExplorerViewModel(IAceCarInfoProvider carInfoProvider)
    {
        this.carInfoProvider = carInfoProvider;
    }

    public ObservableCollection<AceCarInfo> Cars { get; } = [];
    public ObservableCollection<string> Manufacturers { get; } = [];

    [RelayCommand]
    private void ExportCsv()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ace-cars.csv");

        using var streamWriter = new StreamWriter(filePath,
            options: new FileStreamOptions
            {
                Access = FileAccess.Write,
                Mode = FileMode.Create
            });

        foreach(var aceCarInfo in this.carInfoProvider.GetCarInfos())
        {
            var fullName = string.IsNullOrEmpty(aceCarInfo.Variant)
                ? $"{aceCarInfo.Manufacturer} {aceCarInfo.Name}"
                : $"{aceCarInfo.Manufacturer} {aceCarInfo.Variant}";

            streamWriter.WriteLine(string.Join(",",
                CsvField(fullName),
                CsvField(aceCarInfo.RacingClass),
                CsvField(aceCarInfo.Year.ToString()),
                CsvField(aceCarInfo.Manufacturer),
                CsvField(aceCarInfo.ModelId)));
            streamWriter.Flush();
        }
    }

    private static string CsvField(string value) =>
        $"\"{value?.Replace("\"", "\"\"")}\"";

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

        foreach(var aceCarInfo in this.carInfoProvider.GetCarInfosForManufacturer(value))
        {
            this.Cars.Add(aceCarInfo);
        }

        if(this.Cars.Count > 0)
        {
            this.SelectedCar = this.Cars[0];
        }
    }
}
