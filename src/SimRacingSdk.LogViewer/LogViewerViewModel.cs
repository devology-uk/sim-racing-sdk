using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimRacingSdk.LogViewer;

public partial class LogViewerViewModel : ObservableObject
{
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

    private string logFolderPath = null!;

    public ObservableCollection<LogFileEntry> LogEntries { get; } = [];
    public ObservableCollection<LogEntryProperty> LogEntryProperties { get; } = [];
    public ObservableCollection<LogFileItem> LogFiles { get; } = [];
    public ObservableCollection<LogFolderItem> LogFolders { get; } = [];
    public ObservableCollection<MessageTypeItem> MessageTypes { get; } = [];

    [RelayCommand]
    private void ApplyFilter()
    {
        this.ShowCurrentPage();
    }

    [RelayCommand]
    private void ClearFilter()
    {
        this.Filter = string.Empty;
    }

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

    public void Init(string logFolderPath)
    {
        this.logFolderPath = logFolderPath;
        this.LoadLogs();
    }

    private void AddLogEntryProperty(string currentToken, string nestingPrefix)
    {
        if(string.IsNullOrWhiteSpace(currentToken))
        {
            return;
        }

        var propertyElements = currentToken.Split('=', StringSplitOptions.TrimEntries);
        this.LogEntryProperties.Add(new LogEntryProperty($"{nestingPrefix}{propertyElements[0]}",
            propertyElements[1]));
    }

    private bool CanExecuteNextPage()
    {
        return this.CurrentPage < this.PageCount;
    }

    private bool CanExecutePreviousPage()
    {
        return this.CurrentPage > 1;
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

    partial void OnFilterChanged(string? value)
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
        this.MessageTypes.Clear();
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

            var messageTypes = this.allLogEntries.Select(e => e.ContentType)
                                   .Distinct()
                                   .ToList();
            foreach(var messageType in messageTypes)
            {
                this.MessageTypes.Add(new MessageTypeItem(messageType));
            }

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
            var content = value.Content;
            if(content.Contains("|"))
            {
                var contentElements = content.Split("|",
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                content = contentElements[^1];
            }

            this.LogEntryProperties.Add(new LogEntryProperty("Content", content));
            return;
        }

        this.ParseContent(value.Content);
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

    private void ParseContent(string content)
    {
        var contentStartIndex = content.IndexOf('{');
        var contentEndIndex = content.LastIndexOf('}');
        var propertiesContent = content[(contentStartIndex + 1)..(contentEndIndex - 1)]
            .Trim();

        var currentToken = string.Empty;
        var nestingPrefix = string.Empty;
        var outputNextComma = false;
        foreach(var character in propertiesContent)
        {
            if(character != ',' && character != '{' && character != '}')
            {
                currentToken += character;
            }

            switch(character)
            {
                case ',' when outputNextComma:
                    currentToken += character;
                    break;
                case ',':
                {
                    if(string.IsNullOrWhiteSpace(currentToken))
                    {
                        break;
                    }

                    this.AddLogEntryProperty(currentToken, nestingPrefix);
                    currentToken = string.Empty;
                    break;
                }
                case '<':
                    outputNextComma = true;
                    break;
                case '>':
                    outputNextComma = false;
                    this.AddLogEntryProperty(currentToken, nestingPrefix);
                    currentToken = string.Empty;
                    break;
                case '{':
                {
                    this.AddLogEntryProperty(currentToken, nestingPrefix);
                    nestingPrefix += "    ";
                    currentToken = string.Empty;
                    break;
                }
                case '}':
                {
                    this.AddLogEntryProperty(currentToken, nestingPrefix);
                    currentToken = string.Empty;
                    if(nestingPrefix.Length >= 4)
                    {
                        nestingPrefix = nestingPrefix[0..^4];
                    }

                    break;
                }
            }
        }

        this.AddLogEntryProperty(currentToken, nestingPrefix);
    }

    private string ParseContentType(string content)
    {
        var contentStartIndex = content.IndexOf('{');
        var contentEndIndex = content.LastIndexOf('}');

        if(contentStartIndex == -1 && contentEndIndex == -1)
        {
            return "Text";
        }

        var contentType = content[..contentStartIndex]
            .Trim();
        return contentType.Split(":",
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0];
    }

    private LogFileEntry ParseLogFileEntry(string line)
    {
        var lineElements = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
        var contentType = this.ParseContentType(lineElements[2]);

        return new LogFileEntry(lineElements[0], lineElements[1], line, contentType);
    }

    private void ShowCurrentPage(List<string>? selectedTypes = null)
    {
        var messageTypes = selectedTypes ?? this.MessageTypes.Where(m => m.IsSelected).Select(m => m.Name)
                                                .ToList()!;
        var filteredByMessageType = this.allLogEntries.Where(e => messageTypes.Contains(e.ContentType))
                                        .ToList();
        var filteredEntries = filteredByMessageType;
        if(!string.IsNullOrWhiteSpace(this.Filter) && this.Filter.Length >= 3)
        {
            var filters = this.Filter.Split(",", StringSplitOptions.RemoveEmptyEntries);
            filteredEntries = [];
            foreach(var keywordOrPhrase in filters)
            {
                var matchedEntries =
                    filteredByMessageType.Where(
                        e => e.Content.Contains(keywordOrPhrase, StringComparison.OrdinalIgnoreCase));
                filteredEntries.AddRange(matchedEntries);
            }
        }

        var pageEntries = filteredEntries.Skip(this.PageSize * (this.CurrentPage - 1))
                                         .Take(this.PageSize);
        this.PageCount = (int)Math.Ceiling((double)filteredEntries.Count / this.PageSize);
        if(this.PageCount < 1)
        {
            this.PageCount = 1;
        }

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