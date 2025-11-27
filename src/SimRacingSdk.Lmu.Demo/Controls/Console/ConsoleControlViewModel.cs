using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Lmu.Demo.Abstractions;

namespace SimRacingSdk.Lmu.Demo.Controls.Console;

public partial class ConsoleControlViewModel : ObservableObject
{
    private readonly IDisposable entriesSubscription;
    private readonly ILogger<ConsoleControlViewModel> logger;
    private readonly IList<string> messages = new List<string>();

    [ObservableProperty]
    private string consoleContent = string.Empty;

    public ConsoleControlViewModel(ILogger<ConsoleControlViewModel> logger, IConsoleLog consoleLog)
    {
        this.logger = logger;
        this.entriesSubscription = consoleLog.Entries.Subscribe(this.HandleNewEntry);
        consoleLog.Cleared += this.HandleConsoleLogCleared;
    }

    private void HandleConsoleLogCleared()
    {
        this.logger.LogDebug("Console log cleared.");
        this.messages.Clear();
        this.ConsoleContent = string.Empty;
    }

    private void HandleNewEntry(string entry)
    {
        this.logger.LogDebug(entry);

        this.messages.Add(entry);
        if(this.messages.Count > 100)
        {
            this.messages.RemoveAt(0);
        }

        this.ConsoleContent = string.Join(Environment.NewLine, this.messages);
    }

    ~ConsoleControlViewModel()
    {
        this.entriesSubscription.Dispose();
    }
}