using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// One or more setup rows addressed by a spoken name; a group ("front dampers") sets all its rows at once.
public record VoiceTarget(IReadOnlyList<string> Phrases, string Description, IReadOnlyList<R3eSetupControl> Controls)
{
    public bool IsGroup => this.Controls.Count > 1;
}
