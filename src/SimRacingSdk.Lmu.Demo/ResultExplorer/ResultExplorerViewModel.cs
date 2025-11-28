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
    private string sessionName = null!;
    [ObservableProperty]
    private string sessionType = null!;
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
    [ObservableProperty]
    private string fileDateTime = null!;
    [ObservableProperty]
    private int sessionLaps;
    [ObservableProperty]
    private int sessionMinutes;
    [ObservableProperty]
    private bool sessionFormationAndStart;
    [ObservableProperty]
    private int sessionMostLapsCompleted;

    public ObservableCollection<DriverItem> Drivers { get; } = [];
    public ObservableCollection<string> EventStream { get; } = [];

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
        this.Drivers.Clear();
        this.EventStream.Clear();

        if (value == null)
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
        this.FileDateTime = resultFile.TimeString ?? string.Empty;
        this.TrackVenue = resultFile.TrackVenue;
        this.TrackCourse = resultFile.TrackCourse;
        this.TrackEvent = resultFile.TrackEvent;
        this.TrackLength = resultFile.TrackLength;
        this.TyreMultiplier = resultFile.TireMultiplier;
        this.TyreWarmers = resultFile.TireWarmers != 0;
        this.VehiclesAllowed = resultFile.VehiclesAllowed ?? string.Empty;

        var session = resultFile.Session;
        this.SessionType = session?.SessionType ?? string.Empty;
        this.SessionName = session?.SessionName ?? string.Empty;
        this.SessionDateTime = session?.TimeString ?? string.Empty;
        var drivingSessionLaps = (session?.Laps ?? 0);
        this.SessionLaps = drivingSessionLaps == int.MaxValue? 0: drivingSessionLaps;
        var drivingSessionMinutes = (session?.Minutes ?? 0);
        this.SessionMinutes = drivingSessionMinutes == int.MaxValue? 0: drivingSessionMinutes;
        this.SessionFormationAndStart = (session?.FormationAndStart ?? 0) != 0;
        this.SessionMostLapsCompleted = session?.MostLapsCompleted ?? 0;

        if(session == null)
        {
            return;
        }

        if(session.Drivers.Any())
        {
            foreach(var driver in session.Drivers.OrderBy(d => d.Position))
            {
                this.Drivers.Add(new DriverItem(driver));
            }
        }

        if(!session.Stream.Any())
        {
            return;
        }

        foreach(var lmuStreamEvent in session.Stream)
        {
            this.EventStream.Add(lmuStreamEvent.ToString());
        }
    }
}