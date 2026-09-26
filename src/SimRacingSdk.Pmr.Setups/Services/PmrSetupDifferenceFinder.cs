using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Services;

// Both views come from the same car's map, so their tabs, sections, fields and values line up one
// to one and can be walked side by side.
internal static class PmrSetupDifferenceFinder
{
    public static IReadOnlyList<PmrSetupDifference> Between(PmrSetupView first, PmrSetupView second)
    {
        return first.Tabs.Zip(second.Tabs)
                    .SelectMany(tabs => BetweenTabs(tabs.First, tabs.Second))
                    .ToList();
    }

    private static IEnumerable<PmrSetupDifference> BetweenTabs(PmrSetupTabView first, PmrSetupTabView second)
    {
        return first.Sections.Zip(second.Sections)
                    .SelectMany(sections => BetweenSections(first.Name, sections.First, sections.Second));
    }

    private static IEnumerable<PmrSetupDifference> BetweenSections(string tabName, PmrSetupSectionView first, PmrSetupSectionView second)
    {
        return first.Fields.Zip(second.Fields)
                    .SelectMany(fields => BetweenFields(tabName, first.Name, fields.First, fields.Second));
    }

    private static IEnumerable<PmrSetupDifference> BetweenFields(string tabName, string sectionName, PmrSetupFieldView first, PmrSetupFieldView second)
    {
        return first.Values.Zip(second.Values)
                    .Where(values => IsChanged(values.First, values.Second))
                    .Select(values => new PmrSetupDifference
                    {
                        ClickDifference = values.Second.Clicks - values.First.Clicks,
                        FieldName = first.Name,
                        First = values.First,
                        Position = values.First.Position,
                        Second = values.Second,
                        SectionName = sectionName,
                        TabName = tabName
                    });
    }

    private static bool IsChanged(PmrSetupValueView first, PmrSetupValueView second)
    {
        return first.RawValue != second.RawValue || first.DisplayText != second.DisplayText;
    }
}
