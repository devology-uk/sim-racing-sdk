namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// How many independent values the game's UI shows for this field - e.g. Tyre Pressure is
// PerCorner (FL/FR/RL/RR each adjustable), Anti-Roll Bar is FrontRear (one value per axle),
// Fuel Level is Single (one value for the whole car). Front/Rear are one axle's left and right
// corners sharing a range ("{Corner}" expands to FL/FR or RL/RR only), for fields whose front and
// rear ranges differ - the game adjusts both sides symmetrically by default.
// Serialized as integers, so new members are only ever appended.
public enum SetupFieldScope
{
    Single,
    FrontRear,
    PerCorner,
    Front,
    Rear
}
