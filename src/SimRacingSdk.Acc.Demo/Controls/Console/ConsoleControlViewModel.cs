using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Demo.Abstractions;

namespace SimRacingSdk.Acc.Demo.Controls.Console;

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