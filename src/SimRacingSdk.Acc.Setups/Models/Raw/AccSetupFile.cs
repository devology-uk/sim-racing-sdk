#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

// Direct deserialization target for ACC's own setup .json file format - field names and
// nesting match ACC's schema exactly (case-insensitive JSON matching handles ACC's own
// inconsistent casing, e.g. "eCUMap", "aRBFront").
public class AccSetupFile
{
    public string CarName { get; set; }
    public AccBasicSetup BasicSetup { get; set; } = new();
    public AccAdvancedSetup AdvancedSetup { get; set; } = new();
    public int TrackBopType { get; set; }
}
