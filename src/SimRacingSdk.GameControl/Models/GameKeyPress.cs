namespace SimRacingSdk.GameControl.Models;

public record GameKeyPress(int VirtualKey, IReadOnlyList<int> ModifierVirtualKeys)
{
    // Games poll input once per frame, so an instant down/up pair can fall between two frames and be missed.
    public TimeSpan HoldDuration { get; init; } = TimeSpan.FromMilliseconds(50);
}
