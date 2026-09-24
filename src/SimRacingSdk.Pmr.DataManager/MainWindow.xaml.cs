using System.Windows;
using System.Windows.Threading;
using SimRacingSdk.Pmr.DataManager.Session;

namespace SimRacingSdk.Pmr.DataManager;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer placementSaveTimer = new() { Interval = TimeSpan.FromMilliseconds(500) };
    private readonly ISessionStateStore sessionStateStore;

    public MainWindow(ISessionStateStore sessionStateStore)
    {
        this.sessionStateStore = sessionStateStore;
        this.InitializeComponent();
        this.RestorePlacement();

        this.placementSaveTimer.Tick += this.OnPlacementSaveTimerTick;
        this.LocationChanged += this.OnPlacementChanged;
        this.SizeChanged += this.OnPlacementChanged;
        this.StateChanged += this.OnPlacementChanged;
    }

    // Debounced - dragging or resizing fires these many times a second.
    private void OnPlacementChanged(object? sender, EventArgs args)
    {
        this.placementSaveTimer.Stop();
        this.placementSaveTimer.Start();
    }

    private void OnPlacementSaveTimerTick(object? sender, EventArgs args)
    {
        this.placementSaveTimer.Stop();
        this.SavePlacement();
    }

    private void RestorePlacement()
    {
        var state = this.sessionStateStore.State;
        if(state.WindowLeft is not { } left || state.WindowTop is not { } top
           || state.WindowWidth is not { } width || state.WindowHeight is not { } height)
        {
            return;
        }

        var savedBounds = new Rect(left, top, width, height);
        if(!IsOnScreen(savedBounds))
        {
            return;
        }

        this.WindowStartupLocation = WindowStartupLocation.Manual;
        this.Left = savedBounds.Left;
        this.Top = savedBounds.Top;
        this.Width = savedBounds.Width;
        this.Height = savedBounds.Height;
        this.WindowState = state.WindowIsMaximized ? WindowState.Maximized : WindowState.Normal;
    }

    private void SavePlacement()
    {
        if(this.WindowState == WindowState.Minimized)
        {
            return;
        }

        var bounds = this.RestoreBounds;
        if(bounds.IsEmpty)
        {
            return;
        }

        var state = this.sessionStateStore.State;
        state.WindowLeft = bounds.Left;
        state.WindowTop = bounds.Top;
        state.WindowWidth = bounds.Width;
        state.WindowHeight = bounds.Height;
        state.WindowIsMaximized = this.WindowState == WindowState.Maximized;
        this.sessionStateStore.Save();
    }

    // Guards against a monitor that was unplugged since the last run leaving the window unreachable.
    private static bool IsOnScreen(Rect bounds)
    {
        var virtualScreen = new Rect(
            SystemParameters.VirtualScreenLeft,
            SystemParameters.VirtualScreenTop,
            SystemParameters.VirtualScreenWidth,
            SystemParameters.VirtualScreenHeight);
        return virtualScreen.IntersectsWith(bounds);
    }
}
