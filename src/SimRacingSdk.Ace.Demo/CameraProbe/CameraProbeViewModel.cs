using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.Demo.Abstractions;
using SimRacingSdk.Ace.SharedMemory.Abstractions;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.GameControl.Abstractions;
using SimRacingSdk.GameControl.Models;

namespace SimRacingSdk.Ace.Demo.CameraProbe;

public partial class CameraProbeViewModel : ObservableObject, IDisposable
{
    private const int ControlVirtualKey = 0x11;
    private const int ShiftVirtualKey = 0x10;
    private const int AltVirtualKey = 0x12;

    private static readonly TimeSpan GameWindowPollInterval = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan GraphicsPollInterval = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan SettleAfterActivation = TimeSpan.FromMilliseconds(300);

    private readonly IConsoleLog consoleLog;
    private readonly IGameKeyboard gameKeyboard;
    private readonly IGameWindowActivator gameWindowActivator;
    private readonly IGameWindowFinder gameWindowFinder;
    private readonly ILogger<CameraProbeViewModel> logger;
    private readonly IAceSharedMemoryProvider sharedMemoryProvider;
    private readonly CompositeDisposable subscriptionSink = new();

    private GameWindow? gameWindow;
    private AceGraphicsData? latestGraphicsData;

    [ObservableProperty]
    private string activeCars = "-";

    [ObservableProperty]
    private string focusedCar = "-";

    [ObservableProperty]
    private string gameStatus = "ACE is not running";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendKeyCommand))]
    private bool isAceWindowFound;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendKeyCommand))]
    private bool isSending;

    [ObservableProperty]
    private bool isAltPressed;

    [ObservableProperty]
    private bool isControlPressed;

    [ObservableProperty]
    private bool isShiftPressed;

    [ObservableProperty]
    private string playerCar = "-";

    [ObservableProperty]
    private int pressDelayMs = 1000;

    [ObservableProperty]
    private int repeatCount = 1;

    [ObservableProperty]
    private Key selectedKey = Key.F1;

    public CameraProbeViewModel(ILogger<CameraProbeViewModel> logger,
        IConsoleLog consoleLog,
        IAceSharedMemoryProvider sharedMemoryProvider,
        IGameWindowFinder gameWindowFinder,
        IGameWindowActivator gameWindowActivator,
        IGameKeyboard gameKeyboard)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.sharedMemoryProvider = sharedMemoryProvider;
        this.gameWindowFinder = gameWindowFinder;
        this.gameWindowActivator = gameWindowActivator;
        this.gameKeyboard = gameKeyboard;
    }

    public IReadOnlyList<Key> Keys => ProbeKeys.All;

    public void Init()
    {
        var uiContext = SynchronizationContext.Current!;
        this.subscriptionSink.Add(Observable.Interval(GameWindowPollInterval)
                                            .StartWith(0L)
                                            .Select(_ => this.gameWindowFinder.Find(AceGameDetector.ProcessName))
                                            .ObserveOn(uiContext)
                                            .Subscribe(this.OnNextGameWindow));
        this.subscriptionSink.Add(Observable.Interval(GraphicsPollInterval)
                                            .Select(_ => this.sharedMemoryProvider.ReadGraphicsData())
                                            .ObserveOn(uiContext)
                                            .Subscribe(this.OnNextGraphicsData));
        this.Log("Camera probe started. Pick a key, then Send to ACE. Focus changes are logged as they happen.");
    }

    public void Dispose()
    {
        this.subscriptionSink.Dispose();
        GC.SuppressFinalize(this);
    }

    private bool CanSendKey()
    {
        return this.IsAceWindowFound && !this.IsSending;
    }

    [RelayCommand(CanExecute = nameof(CanSendKey))]
    private async Task SendKey()
    {
        this.IsSending = true;
        try
        {
            if(await this.ActivateGameWindow())
            {
                await this.PressRepeatedly(this.CreateKeyPress());
            }
        }
        finally
        {
            this.IsSending = false;
        }
    }

    [RelayCommand]
    private void LogCarTable()
    {
        if(this.latestGraphicsData is not { IsEmpty: false } graphicsData)
        {
            this.Log("No ACE graphics data yet - is a session loaded?");
            return;
        }

        this.Log(CarTableFormatter.Format(graphicsData));
    }

    private async Task<bool> ActivateGameWindow()
    {
        if(this.gameWindowActivator.IsActive(this.gameWindow!))
        {
            return true;
        }

        if(!await this.gameWindowActivator.ActivateAsync(this.gameWindow!))
        {
            this.Log("Couldn't bring the ACE window to the front, nothing was sent.", LogLevel.Warning);
            return false;
        }

        this.Log("ACE window activated.");
        await Task.Delay(SettleAfterActivation);
        return true;
    }

    private async Task PressRepeatedly(GameKeyPress keyPress)
    {
        for(var pressNumber = 1; pressNumber <= this.RepeatCount; pressNumber++)
        {
            var result = await this.gameKeyboard.PressAsync(this.gameWindow!, keyPress);
            if(result != GameKeyPressResult.Sent)
            {
                this.Log($"Press {pressNumber} not sent: {result}. Stopping.", LogLevel.Warning);
                return;
            }

            await Task.Delay(this.PressDelayMs);
            this.Log($"Press {pressNumber} of {this.RepeatCount} ({this.DescribeKey()}) sent. Focused car now {this.FocusedCar}");
        }
    }

    private GameKeyPress CreateKeyPress()
    {
        return new GameKeyPress(KeyInterop.VirtualKeyFromKey(this.SelectedKey), this.SelectedModifiers());
    }

    private List<int> SelectedModifiers()
    {
        var modifiers = new List<int>();
        if(this.IsControlPressed)
        {
            modifiers.Add(ControlVirtualKey);
        }

        if(this.IsShiftPressed)
        {
            modifiers.Add(ShiftVirtualKey);
        }

        if(this.IsAltPressed)
        {
            modifiers.Add(AltVirtualKey);
        }

        return modifiers;
    }

    private string DescribeKey()
    {
        var modifiers = new[]
        {
            this.IsControlPressed ? "Ctrl" : null,
            this.IsShiftPressed ? "Shift" : null,
            this.IsAltPressed ? "Alt" : null
        };
        return string.Join("+", modifiers.OfType<string>().Append(this.SelectedKey.ToString()));
    }

    private string DescribeCar(AceGraphicsData graphicsData, AceCarIdPair carId)
    {
        var index = CarTableFormatter.FindCarIndex(graphicsData, carId);
        return $"[{index}] {carId.A}/{carId.B}";
    }

    private void OnNextGameWindow(GameWindow? foundWindow)
    {
        this.gameWindow = foundWindow;
        if(this.IsAceWindowFound == (foundWindow != null))
        {
            return;
        }

        this.IsAceWindowFound = foundWindow != null;
        this.GameStatus = this.IsAceWindowFound ? "ACE is running" : "ACE is not running";
        this.Log(this.GameStatus);
    }

    private void OnNextGraphicsData(AceGraphicsData graphicsData)
    {
        var previousGraphicsData = this.latestGraphicsData;
        this.latestGraphicsData = graphicsData;
        if(graphicsData.IsEmpty)
        {
            return;
        }

        this.FocusedCar = this.DescribeCar(graphicsData, graphicsData.FocusedCarId);
        this.PlayerCar = this.DescribeCar(graphicsData, graphicsData.PlayerCarId);
        this.ActiveCars = graphicsData.ActiveCars.ToString();
        this.LogFocusChange(previousGraphicsData, graphicsData);
    }

    private void LogFocusChange(AceGraphicsData? previousGraphicsData, AceGraphicsData graphicsData)
    {
        if(previousGraphicsData is not { IsEmpty: false }
           || previousGraphicsData.FocusedCarId == graphicsData.FocusedCarId)
        {
            return;
        }

        this.Log($"{DateTime.Now:HH:mm:ss.fff} Focus changed to {this.FocusedCar}");
    }

    private void Log(string message, LogLevel logLevel = LogLevel.Information)
    {
        this.logger.Log(logLevel, message);
        this.consoleLog.Write(message);
    }
}
