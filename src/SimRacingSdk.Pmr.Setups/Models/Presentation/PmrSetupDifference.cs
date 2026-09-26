namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

public record PmrSetupDifference
{
    // Second minus first, in clicks. Null where either side has no saved value to count from.
    public int? ClickDifference { get; init; }

    public required string FieldName { get; init; }
    public required PmrSetupValueView First { get; init; }
    public required PmrSetupPosition Position { get; init; }
    public required PmrSetupValueView Second { get; init; }
    public required string SectionName { get; init; }
    public required string TabName { get; init; }
}
