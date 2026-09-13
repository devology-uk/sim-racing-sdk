using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.LogViewer;
using SimRacingSdk.Pmr.Demo.Abstractions;
using SimRacingSdk.Pmr.Demo.CarExplorer;
using SimRacingSdk.Pmr.Demo.TrackExplorer;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SimRacingSdk.Pmr.Demo;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IConsoleLog consoleLog;
    private readonly string logFolderPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\";
    private readonly IUdpDemo udpDemo;

    [ObservableProperty]
    private bool isRunningDemo;

    public MainWindowViewModel(IConsoleLog consoleLog, IUdpDemo udpDemo)
    {
        this.consoleLog = consoleLog;
        this.udpDemo = udpDemo;
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
    private void StartUdpDemo()
    {
        this.consoleLog.Clear();
        this.StopRunningDemos();
        this.IsRunningDemo = true;

        if(!this.udpDemo.Validate())
        {
            this.IsRunningDemo = false;
            return;
        }

        this.udpDemo.Start();
    }

    [RelayCommand]
    private void StopDemo()
    {
        this.StopRunningDemos();
    }

    private void StopRunningDemos()
    {
        this.udpDemo.Stop();
        this.IsRunningDemo = false;
    }

    ~MainWindowViewModel()
    {
        this.StopRunningDemos();
    }
}
