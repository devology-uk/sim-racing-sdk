namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// How many independent values the game's UI shows for this field - e.g. Tyre Pressure is
// PerCorner (FL/FR/RL/RR each adjustable), Anti-Roll Bar is FrontRear (one value per axle),
// Fuel Level is Single (one value for the whole car).
public enum SetupFieldScope
{
    Single,
    FrontRear,
    PerCorner
}
