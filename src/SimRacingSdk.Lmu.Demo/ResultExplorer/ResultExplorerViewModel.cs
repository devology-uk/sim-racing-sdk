using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Demo.ResultExplorer;

public partial class ResultExplorerViewModel : ObservableObject
{
    private readonly ILmuGameDataProvider lmuGameDataProvider;
    private readonly ILogger<ResultExplorerViewModel> logger;

    [ObservableProperty]
    private bool clientFuelVisible;
    [ObservableProperty]
    private int damageMultiplier;
    [ObservableProperty]
    private bool dedicated;
    [ObservableProperty]
    private bool fixedSetup;
    [ObservableProperty]
    private bool fixedUpgrades;
    [ObservableProperty]
    private bool freeSettings;
    [ObservableProperty]
    private int fuelMultiplier;
    [ObservableProperty]
    private string gameVersion = null!;
    [ObservableProperty]
    private bool limitedTyres;
    [ObservableProperty]
    private int mechanicalFailRate;
    [ObservableProperty]
    private int parcFerme;
    [ObservableProperty]
    private int raceLaps;
    [ObservableProperty]
    private int raceTime;
    [ObservableProperty]
    private ResultFileItem? selectedResult;
    [ObservableProperty]
    private string serverName = null!;
    [ObservableProperty]
    private string? sessionDateTime;
    [ObservableProperty]
    private string setting = null!;
    [ObservableProperty]
    private string? trackCourse;
    [ObservableProperty]
    private string? trackEvent;
    [ObservableProperty]
    private double trackLength;
    [ObservableProperty]
    private string? trackVenue;
    [ObservableProperty]
    private int tyreMultiplier;
    [ObservableProperty]
    private bool tyreWarmers;
    [ObservableProperty]
    private string vehiclesAllowed = null!;

    public ResultExplorerViewModel(ILogger<ResultExplorerViewModel> logger,
        ILmuGameDataProvider lmuGameDataProvider)
    {
        this.logger = logger;
        this.lmuGameDataProvider = lmuGameDataProvider;
    }

    public ObservableCollection<ResultFileItem> ResultFiles { get; } = [];

    internal void Init()
    {
        this.LoadResultFiles();
    }

    private void LoadResultFiles()
    {
        var resultFiles = this.lmuGameDataProvider.ListResultFiles();
        foreach(var resultFile in resultFiles)
        {
            this.ResultFiles.Add(new ResultFileItem(Path.GetFileNameWithoutExtension(resultFile),
                resultFile));
        }

        this.SelectedResult = this.ResultFiles[0];
    }

    partial void OnSelectedResultChanged(ResultFileItem? value)
    {
        if(value == null)
        {
            return;
        }

        var resultFile = LmuSessionFile.Load(value.FilePath);
        this.ClientFuelVisible = resultFile.ClientFuelVisible != 0;
        this.DamageMultiplier = resultFile.DamageMultiplier;
        this.Dedicated = resultFile.Dedicated != 0;
        this.FixedSetup = resultFile.FixedSetup != 0;
        this.FixedUpgrades = resultFile.FixedUpgrades != 0;
        this.FreeSettings = resultFile.FreeSettings != 0;
        this.FuelMultiplier = resultFile.FuelMultiplier;
        this.GameVersion = resultFile.GameVersion ?? string.Empty;
        this.LimitedTyres = resultFile.LimitedTyres != 0;
        this.MechanicalFailRate = resultFile.MechanicalFailRate;
        this.ParcFerme = resultFile.ParcFerme;
        this.RaceLaps = resultFile.RaceLaps;
        this.RaceTime = resultFile.RaceTime;
        this.ServerName = resultFile.ServerName ?? string.Empty;
        this.SessionDateTime = resultFile.TimeString;
        this.TrackVenue = resultFile.TrackVenue;
        this.TrackCourse = resultFile.TrackCourse;
        this.TrackEvent = resultFile.TrackEvent;
        this.TrackLength = resultFile.TrackLength;
        this.TyreMultiplier = resultFile.TireMultiplier;
        this.TyreWarmers = resultFile.TireWarmers != 0;
        this.VehiclesAllowed = resultFile.VehiclesAllowed ?? string.Empty;
    }
}