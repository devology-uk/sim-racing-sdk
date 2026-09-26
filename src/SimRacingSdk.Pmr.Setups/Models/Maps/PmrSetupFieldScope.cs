namespace SimRacingSdk.Pmr.Setups.Models.Maps;

// Which values the game's screen shows for a field: one for the car, one per axle, one per corner,
// or one axle's two corners (a field whose front and rear ranges differ is two rows, Front and Rear).
public enum PmrSetupFieldScope
{
    Single,
    FrontRear,
    PerCorner,
    Front,
    Rear
}
