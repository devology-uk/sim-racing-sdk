namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public interface IDefaultSetupFieldApplier
{
    // Adds any DefaultSetupFields entry not already present (matched by Name, per tab), leaving
    // existing fields untouched. Only meant for a car being started - on a finished map it would
    // re-add rows deliberately deleted because the game shows "-" for them. Returns how many were added.
    int AddMissingFields(PmrSetupMap map);

    // Makes the Force Feedback section match the defaults exactly - it's identical on every car.
    // Returns how many rows were added or replaced.
    int ApplyForceFeedback(PmrSetupMap map);
}
