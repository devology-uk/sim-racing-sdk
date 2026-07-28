using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Demo.CarExplorer;

// Flat list, no class grouping - unlike Acc's cars.json, Evo's cars.json has no
// class/manufacturer/year/fuel/rpm fields, only Name and DisplayName.
public partial class CarExplorerViewModel : ObservableObject
{
    private readonly IAceCarInfoProvider carInfoProvider;

    [ObservableProperty]
    private AceCarInfo? selectedCar = null;

    public CarExplorerViewModel(IAceCarInfoProvider carInfoProvider)
    {
        this.carInfoProvider = carInfoProvider;
    }

    public ObservableCollection<AceCarInfo> Cars { get; } = [];

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
            streamWriter.WriteLine($"{aceCarInfo.Name},{aceCarInfo.DisplayName}");
            streamWriter.Flush();
        }
    }

    internal void Init()
    {
        this.Cars.Clear();
        foreach(var aceCarInfo in this.carInfoProvider.GetCarInfos())
        {
            this.Cars.Add(aceCarInfo);
        }

        if(this.Cars.Count > 0)
        {
            this.SelectedCar = this.Cars[0];
        }
    }
}
