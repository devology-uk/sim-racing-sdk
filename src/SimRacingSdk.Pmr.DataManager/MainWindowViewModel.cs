using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.LogViewer;
using SimRacingSdk.Pmr.DataManager.Cars;
using SimRacingSdk.Pmr.DataManager.Navigation;
using SimRacingSdk.Pmr.DataManager.Session;
using SimRacingSdk.Pmr.DataManager.SetupMaps;
using SimRacingSdk.Pmr.DataManager.Tracks;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SimRacingSdk.Pmr.DataManager;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly string logFolderPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\";
    private readonly ISessionStateStore sessionStateStore;

    [ObservableProperty]
    private object? currentPageViewModel;

    [ObservableProperty]
    private NavigationItem? selectedNavigationItem;

    public MainWindowViewModel(
        CarsViewModel carsViewModel,
        TracksViewModel tracksViewModel,
        SetupMapsViewModel setupMapsViewModel,
        ISessionStateStore sessionStateStore)
    {
        this.sessionStateStore = sessionStateStore;
        this.NavigationItems =
        [
            new NavigationItem { Title = "Cars", PageViewModel = carsViewModel },
            new NavigationItem { Title = "Tracks", PageViewModel = tracksViewModel },
            new NavigationItem { Title = "Setup Maps", PageViewModel = setupMapsViewModel }
        ];

        this.SelectedNavigationItem = this.NavigationItems.FirstOrDefault(item => item.Title == sessionStateStore.State.Page)
                                      ?? this.NavigationItems[0];
    }

    public IReadOnlyList<NavigationItem> NavigationItems { get; }

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

    partial void OnSelectedNavigationItemChanged(NavigationItem? value)
    {
        this.CurrentPageViewModel = value?.PageViewModel;
        this.sessionStateStore.State.Page = value?.Title;
        this.sessionStateStore.Save();
    }
}
