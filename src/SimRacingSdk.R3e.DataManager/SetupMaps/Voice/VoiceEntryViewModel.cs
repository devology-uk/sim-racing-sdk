using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.R3e.DataManager.SetupMaps.Voice.Commands;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// Hands-free map entry: everything is said back, so the game never has to leave the screen.
public partial class VoiceEntryViewModel : ObservableObject, IVoiceSession
{
    private const string AgainReply = "Say again.";
    private const string ListeningStatus = "Listening. Say \"help\" for the commands.";

    // Below the first, a phrase is taken for background noise and ignored; below the second, it's asked for again.
    private const float LeastHeardConfidence = 0.3f;
    private const float LeastActedOnConfidence = 0.5f;

    private readonly IReadOnlyList<string> carNames;
    private readonly VoiceCommandContext context;
    private readonly IVoiceListener listener;
    private readonly ISetupMapNavigator navigator;
    private readonly IVoiceSpeaker speaker;
    private readonly Stack<CapturedSetupMap> undoSnapshots = new();

    private bool isPaused;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ToggleLabel))]
    private bool isListening;

    [ObservableProperty]
    private string lastHeard = string.Empty;

    [ObservableProperty]
    private string lastReply = string.Empty;

    [ObservableProperty]
    private string status = "Voice entry is off.";

    public VoiceEntryViewModel(IVoiceListener listener, IVoiceSpeaker speaker, ISetupMapNavigator navigator, IReadOnlyList<string> carNames)
    {
        this.listener = listener;
        this.speaker = speaker;
        this.navigator = navigator;
        this.carNames = carNames;
        this.context = new VoiceCommandContext(navigator, this);
    }

    public string ToggleLabel => this.IsListening ? "Stop Voice Entry" : "Start Voice Entry";

    public string Pause()
    {
        this.isPaused = true;
        this.Status = "Paused. Say \"resume listening\".";
        return "Paused.";
    }

    public string Resume()
    {
        this.isPaused = false;
        this.Status = ListeningStatus;
        return "Listening.";
    }

    // Snapshots from another car are dropped: undo only ever changes the car being edited.
    public string Undo()
    {
        while(this.undoSnapshots.Count > 0)
        {
            var snapshot = this.undoSnapshots.Pop();
            if(snapshot.CarGameId == this.navigator.Editor.CarGameId)
            {
                this.navigator.Editor.Restore(snapshot);
                return "Undone.";
            }
        }

        return "Nothing to undo.";
    }

    [RelayCommand]
    private void ToggleListening()
    {
        if(this.IsListening)
        {
            this.StopListening();
            return;
        }

        this.StartListening();
    }

    private void Act(HeardPhrase heard, VoiceCommand? command)
    {
        if(command is null || heard.Confidence < LeastActedOnConfidence)
        {
            this.Reply(AgainReply);
            return;
        }

        if(command.ChangesMap)
        {
            this.undoSnapshots.Push(this.navigator.Editor.ToSetupMap());
        }

        this.Reply(command.Execute(this.context));
    }

    private void OnHeard(object? sender, HeardPhrase heard)
    {
        if(heard.Confidence < LeastHeardConfidence || this.speaker.WasSpeakingSince(heard.StartedAt))
        {
            return;
        }

        var command = VoiceCommandParser.Parse(heard);
        if(this.isPaused && command?.WorksWhilePaused != true)
        {
            return;
        }

        this.LastHeard = $"{string.Join(' ', heard.Words)} ({heard.Confidence:P0})";
        this.Act(heard, command);
    }

    private void Reply(string reply)
    {
        this.LastReply = reply;
        this.speaker.Say(reply);
    }

    private void StartListening()
    {
        try
        {
            this.listener.Heard += this.OnHeard;
            this.listener.Start(culture => VoiceGrammar.Build(culture, this.carNames));
            this.IsListening = true;
            this.isPaused = false;
            this.Status = ListeningStatus;
            this.Reply("Listening.");
        }
        catch(VoiceUnavailableException exception)
        {
            this.listener.Heard -= this.OnHeard;
            this.Status = exception.Message;
        }
    }

    private void StopListening()
    {
        this.listener.Heard -= this.OnHeard;
        this.listener.Stop();
        this.IsListening = false;
        this.Status = "Voice entry is off.";
    }
}
