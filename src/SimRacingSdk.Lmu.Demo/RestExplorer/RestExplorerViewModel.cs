using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Lmu.Demo.Abstractions;
using SimRacingSdk.Lmu.Rest;
using SimRacingSdk.Lmu.Rest.Abstractions;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

public partial class RestExplorerViewModel : ObservableObject, IDisposable
{
    private static readonly TimeSpan SettleAfterAction = TimeSpan.FromMilliseconds(500);

    private static readonly string[] CaptureStateLabels =
    [
        "Main menu",
        "Garage",
        "Driving on track",
        "Race with AI",
        "Spectating (Monitor)",
        "Replay"
    ];

    private static readonly string[] SnapshotPaths =
    [
        "/rest/watch/focus",
        "/rest/replay/CameraController/getCameraInfo",
        "/rest/replay/isActive"
    ];

    private readonly ApiResponsivenessWatch apiResponsivenessWatch;
    private readonly IConsoleLog consoleLog;
    private readonly ILmuRestEndpointCatalog endpointCatalog;
    private readonly ILogger<RestExplorerViewModel> logger;
    private readonly ILmuRestClientFactory restClientFactory;

    private RestCaptureWriter? captureWriter;
    private ILmuRestClient? restClient;
    private CancellationTokenSource? watchCancellation;

    [ObservableProperty]
    private string baseAddress = LmuRestClient.DefaultBaseAddress;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendSelectedCommand))]
    [NotifyCanExecuteChangedFor(nameof(CaptureAllSafeReadsCommand))]
    private bool isBusy;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartWatchCommand))]
    [NotifyCanExecuteChangedFor(nameof(StopWatchCommand))]
    private bool isWatching;

    [ObservableProperty]
    private bool isSnapshotAfterAction = true;

    [ObservableProperty]
    private string lastResponseText = string.Empty;

    [ObservableProperty]
    private string requestBody = string.Empty;

    [ObservableProperty]
    private string selectedCaptureState = CaptureStateLabels[0];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AcceptsBody))]
    [NotifyCanExecuteChangedFor(nameof(SendSelectedCommand))]
    private LmuRestEndpoint? selectedEndpoint;

    public RestExplorerViewModel(ILogger<RestExplorerViewModel> logger,
        IConsoleLog consoleLog,
        ILmuRestClientFactory restClientFactory,
        ILmuRestEndpointCatalog endpointCatalog,
        ApiResponsivenessWatch apiResponsivenessWatch)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.restClientFactory = restClientFactory;
        this.endpointCatalog = endpointCatalog;
        this.apiResponsivenessWatch = apiResponsivenessWatch;
    }

    public IReadOnlyList<string> CaptureStates => CaptureStateLabels;

    public bool AcceptsBody => this.SelectedEndpoint?.AcceptsBody == true;
    public string CaptureFolderPath => this.captureWriter?.CaptureFolderPath ?? string.Empty;
    public IReadOnlyList<LmuRestEndpoint> Endpoints => this.endpointCatalog.All;
    public ObservableCollection<RouteValueInput> RouteValues { get; } = [];

    public void Init()
    {
        var logsFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "logs", "rest");
        this.captureWriter = new RestCaptureWriter(logsFolderPath);
        this.OnPropertyChanged(nameof(this.CaptureFolderPath));
        this.Log($"REST Explorer started. Every response is saved under {this.CaptureFolderPath}");
        this.Log("Pick the game state you're in, then Capture All Safe Reads. Repeat for each state.");
    }

    public void Dispose()
    {
        this.watchCancellation?.Cancel();
        this.restClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    partial void OnSelectedEndpointChanged(LmuRestEndpoint? value)
    {
        this.RouteValues.Clear();
        foreach(var name in value?.ParameterNames ?? Array.Empty<string>())
        {
            this.RouteValues.Add(new RouteValueInput(name));
        }
    }

    private bool CanSendSelected()
    {
        return this.SelectedEndpoint != null && !this.IsBusy;
    }

    [RelayCommand(CanExecute = nameof(CanSendSelected))]
    private async Task SendSelected()
    {
        await this.RunBusy(async () =>
                           {
                               var endpoint = this.SelectedEndpoint!;
                               var response = await this.Send(endpoint.Method, endpoint.BuildPath(this.ReadRouteValues()),
                                   endpoint.AcceptsBody ? this.RequestBody : null);
                               this.ShowResponse(response);
                               if(endpoint.Kind == LmuRestEndpointKind.Action && this.IsSnapshotAfterAction)
                               {
                                   await this.CaptureSnapshotAfterAction();
                               }
                           });
    }

    private bool CanCaptureAllSafeReads()
    {
        return !this.IsBusy;
    }

    [RelayCommand(CanExecute = nameof(CanCaptureAllSafeReads))]
    private async Task CaptureAllSafeReads()
    {
        await this.RunBusy(async () =>
                           {
                               var safeReads = this.endpointCatalog.SafeReads.ToList();
                               this.Log($"Capturing {safeReads.Count} safe reads for state '{this.SelectedCaptureState}'...");
                               var successCount = 0;
                               foreach(var endpoint in safeReads)
                               {
                                   var response = await this.Send(endpoint.Method, endpoint.PathTemplate);
                                   successCount += response.IsSuccess ? 1 : 0;
                               }

                               this.Log($"Capture for '{this.SelectedCaptureState}' done: {successCount} of {safeReads.Count} succeeded.");
                           });
    }

    private bool CanStartWatch()
    {
        return !this.IsWatching;
    }

    [RelayCommand(CanExecute = nameof(CanStartWatch))]
    private async Task StartWatch()
    {
        this.IsWatching = true;
        this.watchCancellation = new CancellationTokenSource();
        this.Log($"Watching {ApiResponsivenessWatch.WatchedPath} every second. Now minimise LMU, switch away, go to the menus...");
        try
        {
            await this.apiResponsivenessWatch.RunAsync(this.GetClient(), this.Log, this.watchCancellation.Token);
        }
        finally
        {
            this.IsWatching = false;
            this.Log("Watch stopped.");
        }
    }

    private bool CanStopWatch()
    {
        return this.IsWatching;
    }

    [RelayCommand(CanExecute = nameof(CanStopWatch))]
    private void StopWatch()
    {
        this.watchCancellation?.Cancel();
    }

    [RelayCommand]
    private void OpenCaptureFolder()
    {
        if(Directory.Exists(this.CaptureFolderPath))
        {
            Process.Start("explorer.exe", this.CaptureFolderPath);
        }
    }

    private async Task CaptureSnapshotAfterAction()
    {
        await Task.Delay(SettleAfterAction);
        foreach(var path in SnapshotPaths)
        {
            await this.Send(HttpMethod.Get, path, note: "after action");
        }
    }

    private ILmuRestClient GetClient()
    {
        if(this.restClient?.BaseAddress.ToString().TrimEnd('/') == this.BaseAddress.TrimEnd('/'))
        {
            return this.restClient;
        }

        this.restClient?.Dispose();
        this.restClient = this.restClientFactory.Create(this.BaseAddress);
        return this.restClient;
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private Dictionary<string, string> ReadRouteValues()
    {
        return this.RouteValues.ToDictionary(input => input.Name, input => input.Value);
    }

    private async Task RunBusy(Func<Task> work)
    {
        this.IsBusy = true;
        try
        {
            await work();
        }
        catch(Exception exception) when(exception is UriFormatException or InvalidOperationException)
        {
            this.Log($"Couldn't send: {exception.Message}");
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    private async Task<LmuRestResponse> Send(HttpMethod method, string path, string? body = null, string note = "")
    {
        var response = await this.GetClient().SendAsync(method, path, body);
        this.captureWriter!.Write(response, this.SelectedCaptureState, note);
        this.Log(ResponseSummary.Describe(response) + (string.IsNullOrEmpty(note) ? string.Empty : $"  ({note})"));
        return response;
    }

    private void ShowResponse(LmuRestResponse response)
    {
        if(!response.IsResponding)
        {
            this.LastResponseText = $"No response: {response.Error}";
            return;
        }

        JsonFormatter.TryIndent(response.Body, out var text);
        this.LastResponseText = text;
    }
}
