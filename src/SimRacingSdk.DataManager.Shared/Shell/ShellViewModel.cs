using System.Diagnostics;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.DataManager.Shared.Navigation;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.LogViewer;

namespace SimRacingSdk.DataManager.Shared.Shell;

public partial class ShellViewModel : ObservableObject
{
    private readonly string logFolderPath = Path.Combine(AppContext.BaseDirectory, "logs");
    private readonly Func<LogViewerViewModel> logViewerViewModelFactory;
    private readonly ISessionStateStore sessionStateStore;

    [ObservableProperty]
    private object? currentPageViewModel;

    [ObservableProperty]
    private NavigationItem? selectedNavigationItem;

    // Pages appear in the order the game's App registers its NavigationItems.
    public ShellViewModel(
        DataManagerOptions options,
        IEnumerable<NavigationItem> navigationItems,
        ISessionStateStore sessionStateStore,
        Func<LogViewerViewModel> logViewerViewModelFactory)
    {
        this.Title = options.Title;
        this.sessionStateStore = sessionStateStore;
        this.logViewerViewModelFactory = logViewerViewModelFactory;
        this.NavigationItems = navigationItems.ToList();

        this.SelectedNavigationItem = this.NavigationItems.FirstOrDefault(item => item.Title == sessionStateStore.State.Page)
                                      ?? this.NavigationItems.FirstOrDefault();
    }

    public IReadOnlyList<NavigationItem> NavigationItems { get; }
    public string Title { get; }

    [RelayCommand]
    private void OpenLogFolder()
    {
        var currentLogPath = Path.Combine(this.logFolderPath, $"{DateTime.Now.Date:yyyy-MM-dd}");
        if(Directory.Exists(currentLogPath))
        {
            Process.Start("explorer.exe", currentLogPath);
        }
    }

    [RelayCommand]
    private void OpenLogViewer()
    {
        var logViewerViewModel = this.logViewerViewModelFactory();
        var logViewer = new LogViewerWindow
        {
            DataContext = logViewerViewModel,
            Owner = Application.Current.MainWindow
        };
        logViewer.Show();
        logViewerViewModel.Init($@"{this.logFolderPath}\");
    }

    partial void OnSelectedNavigationItemChanged(NavigationItem? value)
    {
        this.CurrentPageViewModel = value?.PageViewModel;
        this.sessionStateStore.State.Page = value?.Title;
        this.sessionStateStore.Save();
    }
}
