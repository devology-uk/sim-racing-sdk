namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice.Commands;

public record UndoCommand : VoiceCommand
{
    public override string Execute(VoiceCommandContext context)
    {
        return context.Session.Undo();
    }
}

public record PauseListeningCommand : VoiceCommand
{
    public override string Execute(VoiceCommandContext context)
    {
        return context.Session.Pause();
    }
}

public record ResumeListeningCommand : VoiceCommand
{
    public override bool WorksWhilePaused => true;

    public override string Execute(VoiceCommandContext context)
    {
        return context.Session.Resume();
    }
}

public record HelpCommand : VoiceCommand
{
    public override string Execute(VoiceCommandContext context)
    {
        return "Say next car, or car and its name. Then a control and a range, like front spring 120 to 220 step 20. "
               + "Or a control and fixed, not fitted or adjustable. Add puts an option or a tooth pair on the end, like gears add 10 to 34. "
               + "Say validated, save, undo, which car, or pause listening.";
    }
}
