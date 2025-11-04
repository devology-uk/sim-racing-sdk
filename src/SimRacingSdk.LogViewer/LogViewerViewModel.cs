using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimRacingSdk.LogViewer;

public partial class LogViewerViewModel : ObservableObject
{
    private string logFolderPath = null!;
    private readonly List<LogFileEntry> allLogEntries = [];

    [ObservableProperty]
    private int currentPage = 1;
    [ObservableProperty]
    private string filter = string.Empty;
    [ObservableProperty]
    private bool isBusy = false;
    [ObservableProperty]
    private int pageCount;
    [ObservableProperty]
    private int pageSize = 50;
    [ObservableProperty]
    private LogFileItem? selectedLog;
    [ObservableProperty]
    private LogFileEntry? selectedLogEntry;
    [ObservableProperty]
    private LogFolderItem? selectedLogFolder;

    public ObservableCollection<LogFileEntry> LogEntries { get; } = [];
    public ObservableCollection<LogEntryProperty> LogEntryProperties { get; } = [];
    public ObservableCollection<LogFileItem> LogFiles { get; } = [];
    public ObservableCollection<LogFolderItem> LogFolders { get; } = [];
    
    [RelayCommand]
    private void DeleteSelectedLogFolder()
    {
        if(this.SelectedLogFolder == null)
        {
            return;
        }

        try
        {
            Directory.Delete(this.SelectedLogFolder.Path, true);
            this.LogFolders.Remove(this.SelectedLogFolder);
            this.SelectedLogFolder = this.LogFolders.Count > 0? this.LogFolders[0]: null;
        }
        catch(Exception exception)
        {
            MessageBox.Show(
                $"An unexpected error occured trying to delete the log folder: {Environment.NewLine}{exception.Message}",
                "Error Deleting Folder",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private bool CanExecutePreviousPage()
    {
        return this.CurrentPage > 1;
    }

    private bool CanExecuteNextPage()
    {
        return this.CurrentPage < this.PageCount;
    }

    [RelayCommand(CanExecute = nameof(CanExecutePreviousPage))]
    private void FirstPage()
    {
        this.CurrentPage = 1;
        this.ShowCurrentPage();
    }

    [RelayCommand(CanExecute = nameof(CanExecuteNextPage))]
    private void LastPage()
    {
        this.CurrentPage = this.PageCount;
        this.ShowCurrentPage();
    }

    [RelayCommand(CanExecute = nameof(CanExecuteNextPage))]
    private void NextPage()
    {
        var nextPage = this.CurrentPage + 1;
        if(nextPage < 1)
        {
            return;
        }

        this.CurrentPage = nextPage;
        this.ShowCurrentPage();
    }

    [RelayCommand(CanExecute = nameof(CanExecutePreviousPage))]
    private void PreviousPage()
    {
        var previousPage = this.CurrentPage - 1;
        if(previousPage < 1)
        {
            return;
        }

        this.CurrentPage = previousPage;
        this.ShowCurrentPage();
    }

    [RelayCommand]
    private void ClearFilter()
    {
        this.Filter = string.Empty;
    }

    public void Init(string logFolderPath)
    {
        this.logFolderPath = logFolderPath;
        this.LoadLogs();
    }

    private void LoadLogs()
    {
        this.IsBusy = true;
        if(!Directory.Exists(this.logFolderPath))
        {
            return;
        }

        var logFolders = Directory.GetDirectories(this.logFolderPath);
        foreach(var logFolder in logFolders)
        {
            var logFolderItem = new LogFolderItem(Path.GetFileNameWithoutExtension(logFolder), logFolder);

            var logFiles = Directory.GetFiles(logFolder, "*.log");
            foreach(var logFile in logFiles)
            {
                var logFileItem = new LogFileItem(Path.GetFileName(logFile), logFile);
                logFolderItem.LogFiles.Add(logFileItem);
            }

            this.LogFolders.Add(logFolderItem);
        }

        if(this.LogFolders.Count > 0)
        {
            this.SelectedLogFolder = this.LogFolders[0];
        }

        this.IsBusy = false;
    }

    partial void OnCurrentPageChanged(int value)
    {
        this.ShowCurrentPage();
    }

    partial void OnPageSizeChanged(int value)
    {
        this.PageCount = (int)Math.Ceiling((double)this.allLogEntries.Count / this.PageSize);
        if(this.CurrentPage > this.PageCount)
        {
            this.CurrentPage = this.PageCount;
        }

        this.ShowCurrentPage();
    }

    partial void OnSelectedLogChanged(LogFileItem? value)
    {
        this.IsBusy = true;
        this.LogEntries.Clear();
        this.allLogEntries.Clear();
        this.SelectedLogEntry = null;

        if(value == null || !File.Exists(value.FilePath))
        {
            return;
        }

        try
        {
            using var fileStream =
                new FileStream(value.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var streamReader = new StreamReader(fileStream);
            while(streamReader.ReadLine() is { } line)
            {
                var logFileEntry = this.ParseLogFileEntry(line);

                this.allLogEntries.Add(logFileEntry);
            }

            this.CurrentPage = 1;
            this.PageCount = (int)Math.Ceiling((double)this.allLogEntries.Count / this.PageSize);
            this.ShowCurrentPage();
        }
        catch(Exception exception)
        {
            MessageBox.Show(
                $"An unexpected error occured trying to open the log file: {Environment.NewLine}{exception.Message}",
                "Error Opening File",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        if(this.LogEntries.Count > 0)
        {
            this.SelectedLogEntry = this.LogEntries[0];
        }

        this.IsBusy = false;
    }

    partial void OnSelectedLogEntryChanged(LogFileEntry? value)
    {
        this.LogEntryProperties.Clear();

        if(value == null)
        {
            return;
        }

        this.LogEntryProperties.Add(new LogEntryProperty("Logged At", value.TimeStamp));
        this.LogEntryProperties.Add(new LogEntryProperty("Level", value.Level));
        this.LogEntryProperties.Add(new LogEntryProperty("Content Type", value.ContentType));

        if(value.ContentType == "Text")
        {
            this.LogEntryProperties.Add(new LogEntryProperty("Content", value.Content));
            return;
        }

        var properties = this.ParseContent(value.Content);
        foreach(var property in properties)
        {
            this.LogEntryProperties.Add(new LogEntryProperty(property.Key, property.Value));
        }
    }

    partial void OnSelectedLogFolderChanged(LogFolderItem? value)
    {
        this.IsBusy = true;
        this.LogFiles.Clear();
        if(value == null)
        {
            this.SelectedLog = null;
            return;
        }

        foreach(var logFile in value.LogFiles)
        {
            this.LogFiles.Add(logFile);
        }

        if(this.LogFiles.Count > 0)
        {
            this.SelectedLog = this.LogFiles[0];
        }

        this.IsBusy = false;
    }

    partial void OnFilterChanged(string? value)
    {
        this.ShowCurrentPage();
    }

    private Dictionary<string, string> ParseContent(string content)
    {
        var contentStartIndex = content.IndexOf('{');
        var contentEndIndex = content.LastIndexOf('}');
        var propertyContent = content[(contentStartIndex + 1)..contentEndIndex]
            .Trim();
        var properties = propertyContent.Split(',', StringSplitOptions.RemoveEmptyEntries);

        var result = new Dictionary<string, string>();
        foreach(var property in properties)
        {
            var propertyElements = property.Split('=', StringSplitOptions.TrimEntries);
            if(!result.ContainsKey(propertyElements[0]))
            {
                result.Add(propertyElements[0],
                    propertyElements.Length == 2? propertyElements[1]: string.Empty);
            }
        }

        return result;
    }

    private string ParseContentType(string content)
    {
        var contentStartIndex = content.IndexOf('{');
        var contentEndIndex = content.LastIndexOf('}');

        if(contentStartIndex == -1 && contentEndIndex == -1)
        {
            return "Text";
        }

        return content[..contentStartIndex]
            .Trim();
    }

    private LogFileEntry ParseLogFileEntry(string line)
    {
        var lineElements = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
        var contentType = this.ParseContentType(lineElements[2]);

        return new LogFileEntry(lineElements[0], lineElements[1], line, contentType);
    }

    private void ShowCurrentPage()
    {
        var filteredEntries = this.allLogEntries;

        if(!string.IsNullOrWhiteSpace(this.Filter) && this.Filter.Length >= 3)
        {
            filteredEntries = filteredEntries
                .Where(entry => entry.Content.Contains((string)this.Filter, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        var pageEntries = Enumerable.Take(filteredEntries.Skip(this.PageSize * (this.CurrentPage - 1)), (int)this.PageSize);
        this.PageCount = (int)Math.Ceiling((double)filteredEntries.Count / this.PageSize);

        this.LogEntries.Clear();
        foreach(var entry in pageEntries)
        {
            this.LogEntries.Add(entry);
        }

        if(this.PageCount < this.CurrentPage)
        {
            this.CurrentPage = this.PageCount;
        }

        if(this.LogEntries.Count > 0)
        {
            this.SelectedLogEntry = this.LogEntries[0];
        }

        this.NextPageCommand.NotifyCanExecuteChanged();
        this.PreviousPageCommand.NotifyCanExecuteChanged();
        this.FirstPageCommand.NotifyCanExecuteChanged();
        this.LastPageCommand.NotifyCanExecuteChanged();
    }
}