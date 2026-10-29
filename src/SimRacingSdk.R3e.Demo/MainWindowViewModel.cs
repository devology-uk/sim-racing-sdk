using System.Diagnostics;
using System.IO;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.LogViewer;
using SimRacingSdk.R3e.Demo.Abstractions;
using SimRacingSdk.R3e.Demo.CarExplorer;
using SimRacingSdk.R3e.Demo.TrackExplorer;

namespace SimRacingSdk.R3e.Demo;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IConsoleLog consoleLog;
    private readonly IGameInfoDemo gameInfoDemo;
    private readonly string logFolderPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\";

    public MainWindowViewModel(IConsoleLog consoleLog, IGameInfoDemo gameInfoDemo)
    {
        this.consoleLog = consoleLog;
        this.gameInfoDemo = gameInfoDemo;
    }

    [RelayCommand]
    private void OpenCarExplorer()
    {
        var carExplorerViewModel = App.Current.Services.GetRequiredService<CarExplorerViewModel>();
        var carExplorer = new CarExplorerWindow
        {
            DataContext = carExplorerViewModel,
            Owner = App.Current.MainWindow
        };
        carExplorer.Show();
        carExplorerViewModel.Init();
    }

    [RelayCommand]
    private void OpenLogFolder()
    {
        var currentLogPath = $@"{this.logFolderPath}{DateTime.Now.Date:yyyy-MM-dd}\";
        if(Directory.Exists(currentLogPath))
        {
            Process.Start("explorer.exe", currentLogPath);
        }
    }

    [RelayCommand]
    private void OpenLogViewer()
    {
        var logViewerViewModel = App.Current.Services.GetRequiredService<LogViewerViewModel>();
        var logViewer = new LogViewerWindow
        {
            DataContext = logViewerViewModel,
            Owner = App.Current.MainWindow
        };
        logViewer.Show();
        logViewerViewModel.Init(this.logFolderPath);
    }

    [RelayCommand]
    private void OpenTrackExplorer()
    {
        var trackExplorerViewModel = App.Current.Services.GetRequiredService<TrackExplorerViewModel>();
        var trackExplorer = new TrackExplorerWindow
        {
            DataContext = trackExplorerViewModel,
            Owner = App.Current.MainWindow
        };
        trackExplorer.Show();
        trackExplorerViewModel.Init();
    }

    [RelayCommand]
    private void ShowGameInfo()
    {
        this.consoleLog.Clear();
        this.gameInfoDemo.Show();
    }
}
