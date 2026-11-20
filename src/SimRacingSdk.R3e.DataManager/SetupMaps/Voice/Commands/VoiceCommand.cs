namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice.Commands;

public abstract record VoiceCommand
{
    // A command that changes the map is snapshotted first, so "undo" can put it back.
    public virtual bool ChangesMap => false;

    public virtual bool WorksWhilePaused => false;

    // Returns what to say back.
    public abstract string Execute(VoiceCommandContext context);
}

public record VoiceCommandContext(ISetupMapNavigator Navigator, IVoiceSession Session)
{
    public SetupMapEditorViewModel Editor => this.Navigator.Editor;

    public IReadOnlyList<SetupControlRowViewModel> RowsOf(VoiceTarget target)
    {
        return target.Controls.Select(this.Editor.RowFor).ToList();
    }
}
