namespace SimRacingSdk.R3e.Setups.Models;

public record R3eSetupMap
{
    // The game's model id, as shared memory reports it.
    public required int CarGameId { get; init; }

    public IReadOnlyList<R3eSetupControlSetting> Controls { get; init; } = [];

    public bool IsAdjustable(R3eSetupControl control)
    {
        return this.SettingFor(control).Availability == R3eSetupControlAvailability.Adjustable;
    }

    public R3eSetupControlSetting SettingFor(R3eSetupControl control)
    {
        return this.Controls.FirstOrDefault(setting => setting.Control == control)
               ?? new R3eSetupControlSetting { Control = control, Availability = R3eSetupControlAvailability.NotFitted };
    }
}
