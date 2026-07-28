using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Enums;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core;

// Same broadcasting protocol as Acc (confirmed 2026-07-27), so the same nationality codes and
// their ISO country codes apply - this table is copied verbatim, not a guess.
public class AceNationalityInfoProvider : IAceNationalityInfoProvider
{
    private static AceNationalityInfoProvider? singletonInstance;

    public List<AceNationalityInfo> Nationalities =
    [
        new(0, "Any", "ZZZ"),
        new((AceNationality)1, "Italy", "ITA"),
        new((AceNationality)2, "Germany", "DEU"),
        new((AceNationality)3, "France", "FRA"),
        new((AceNationality)4, "Spain", "ESP"),
        new((AceNationality)5, "Great Britain", "GBR"),
        new((AceNationality)6, "Hungary", "HUN"),
        new((AceNationality)7, "Belgium", "BEL"),
        new((AceNationality)8, "Switzerland", "CHE"),
        new((AceNationality)9, "Austria", "AUT"),
        new((AceNationality)10, "Russia", "RUS"),
        new((AceNationality)11, "Thailand", "THA"),
        new((AceNationality)12, "Netherlands", "NLD"),
        new((AceNationality)13, "Poland", "POL"),
        new((AceNationality)14, "Argentina", "ARG"),
        new((AceNationality)15, "Monaco", "MCO"),
        new((AceNationality)16, "Ireland", "IRL"),
        new((AceNationality)17, "Brazil", "BRA"),
        new((AceNationality)18, "South Africa", "ZAF"),
        new((AceNationality)19, "Puerto Rico", "PRI"),
        new((AceNationality)20, "Slovakia", "SVK"),
        new((AceNationality)21, "Oman", "OMN"),
        new((AceNationality)22, "Greece", "GRC"),
        new((AceNationality)23, "Saudi Arabia", "SAU"),
        new((AceNationality)24, "Norway", "NOR"),
        new((AceNationality)25, "Turkey", "TUR"),
        new((AceNationality)26, "South Korea", "KOR"),
        new((AceNationality)27, "Lebanon", "LBN"),
        new((AceNationality)28, "Armenia", "ARM"),
        new((AceNationality)29, "Mexico", "MEX"),
        new((AceNationality)30, "Sweden", "SWE"),
        new((AceNationality)31, "Finland", "FIN"),
        new((AceNationality)32, "Denmark", "DNK"),
        new((AceNationality)33, "Croatia", "HRV"),
        new((AceNationality)34, "Canada", "CAN"),
        new((AceNationality)35, "China", "CHN"),
        new((AceNationality)36, "Portugal", "PRT"),
        new((AceNationality)37, "Singapore", "SGP"),
        new((AceNationality)38, "Indonesia", "IDN"),
        new((AceNationality)39, "USA", "USA"),
        new((AceNationality)40, "New Zealand", "NZL"),
        new((AceNationality)41, "Australia", "AUS"),
        new((AceNationality)42, "San Marino", "SMR"),
        new((AceNationality)43, "UAE", "ARE"),
        new((AceNationality)44, "Luxembourg", "LUX"),
        new((AceNationality)45, "Kuwait", "KWT"),
        new((AceNationality)46, "Hong Kong", "HKG"),
        new((AceNationality)47, "Colombia", "COL"),
        new((AceNationality)48, "Japan", "JPN"),
        new((AceNationality)49, "Andorra", "AND"),
        new((AceNationality)50, "Azerbaijan", "AZE"),
        new((AceNationality)51, "Bulgaria", "BGR"),
        new((AceNationality)52, "Cuba", "CUB"),
        new((AceNationality)53, "Czech Republic", "CZE"),
        new((AceNationality)54, "Estonia", "EST"),
        new((AceNationality)55, "Georgia", "GEO"),
        new((AceNationality)56, "India", "IND"),
        new((AceNationality)57, "Israel", "ISR"),
        new((AceNationality)58, "Jamaica", "JAM"),
        new((AceNationality)59, "Latvia", "LVA"),
        new((AceNationality)60, "Lithuania", "LTU"),
        new((AceNationality)61, "Macau", "MAC"),
        new((AceNationality)62, "Malaysia", "MYS"),
        new((AceNationality)63, "Nepal", "NPL"),
        new((AceNationality)64, "New Caledonia", "NCL"),
        new((AceNationality)65, "Nigeria", "NER"),
        new((AceNationality)66, "Northern Ireland", "NIR"),
        new((AceNationality)67, "Papua New Guinea", "PNG"),
        new((AceNationality)68, "Philippines", "PHL"),
        new((AceNationality)69, "Qatar", "QAT"),
        new((AceNationality)70, "Romania", "ROU"),
        new((AceNationality)71, "Scotland", "GBR-SCT"),
        new((AceNationality)72, "Serbia", "SRB"),
        new((AceNationality)73, "Slovenia", "SVK"),
        new((AceNationality)74, "Taiwan", "TWN"),
        new((AceNationality)75, "Ukraine", "UKR"),
        new((AceNationality)76, "Venezuela", "VEN"),
        new((AceNationality)77, "Wales", "GBR-CYM"),
        new((AceNationality)78, "Iran", "IRN"),
        new((AceNationality)79, "Bahrain", "BHR"),
        new((AceNationality)80, "Zimbabwe", "ZWE"),
        new((AceNationality)81, "Chinese Taipei", "CHN"),
        new((AceNationality)82, "Chile", "CHL"),
        new((AceNationality)83, "Uruguay", "URU"),
        new((AceNationality)84, "Madagascar", "MAD"),
        new((AceNationality)86, "England", "GBR-ENG")
    ];

    public static AceNationalityInfoProvider Instance =>
        singletonInstance ??= new AceNationalityInfoProvider();

    public AceNationalityInfo? FindById(AceNationality aceNationality)
    {
        return this.Nationalities.FirstOrDefault(n => n.AceNationality == aceNationality);
    }

    public string GetCountryCode(AceNationality aceNationality)
    {
        return this.FindById(aceNationality)
                   ?.CountryCode ?? "ZZZ";
    }
}
