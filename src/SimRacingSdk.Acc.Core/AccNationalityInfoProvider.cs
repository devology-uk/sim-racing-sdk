using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Core;

public class AccNationalityInfoProvider : IAccNationalityInfoProvider
{
    private static AccNationalityInfoProvider? singletonInstance;

    public List<AccNationalityInfo> Nationalities =
    [
        new(0, "Any", "ZZZ"),
        new((AccNationality)1, "Italy", "ITA"),
        new((AccNationality)2, "Germany", "DEU"),
        new((AccNationality)3, "France", "FRA"),
        new((AccNationality)4, "Spain", "ESP"),
        new((AccNationality)5, "Great Britain", "GBR"),
        new((AccNationality)6, "Hungary", "HUN"),
        new((AccNationality)7, "Belgium", "BEL"),
        new((AccNationality)8, "Switzerland", "CHE"),
        new((AccNationality)9, "Austria", "AUT"),
        new((AccNationality)10, "Russia", "RUS"),
        new((AccNationality)11, "Thailand", "THA"),
        new((AccNationality)12, "Netherlands", "NLD"),
        new((AccNationality)13, "Poland", "POL"),
        new((AccNationality)14, "Argentina", "ARG"),
        new((AccNationality)15, "Monaco", "MCO"),
        new((AccNationality)16, "Ireland", "IRL"),
        new((AccNationality)17, "Brazil", "BRA"),
        new((AccNationality)18, "South Africa", "ZAF"),
        new((AccNationality)19, "Puerto Rico", "PRI"),
        new((AccNationality)20, "Slovakia", "SVK"),
        new((AccNationality)21, "Oman", "OMN"),
        new((AccNationality)22, "Greece", "GRC"),
        new((AccNationality)23, "Saudi Arabia", "SAU"),
        new((AccNationality)24, "Norway", "NOR"),
        new((AccNationality)25, "Turkey", "TUR"),
        new((AccNationality)26, "South Korea", "KOR"),
        new((AccNationality)27, "Lebanon", "LBN"),
        new((AccNationality)28, "Armenia", "ARM"),
        new((AccNationality)29, "Mexico", "MEX"),
        new((AccNationality)30, "Sweden", "SWE"),
        new((AccNationality)31, "Finland", "FIN"),
        new((AccNationality)32, "Denmark", "DNK"),
        new((AccNationality)33, "Croatia", "HRV"),
        new((AccNationality)34, "Canada", "CAN"),
        new((AccNationality)35, "China", "CHN"),
        new((AccNationality)36, "Portugal", "PRT"),
        new((AccNationality)37, "Singapore", "SGP"),
        new((AccNationality)38, "Indonesia", "IDN"),
        new((AccNationality)39, "USA", "USA"),
        new((AccNationality)40, "New Zealand", "NZL"),
        new((AccNationality)41, "Australia", "AUS"),
        new((AccNationality)42, "San Marino", "SMR"),
        new((AccNationality)43, "UAE", "ARE"),
        new((AccNationality)44, "Luxembourg", "LUX"),
        new((AccNationality)45, "Kuwait", "KWT"),
        new((AccNationality)46, "Hong Kong", "HKG"),
        new((AccNationality)47, "Colombia", "COL"),
        new((AccNationality)48, "Japan", "JPN"),
        new((AccNationality)49, "Andorra", "AND"),
        new((AccNationality)50, "Azerbaijan", "AZE"),
        new((AccNationality)51, "Bulgaria", "BGR"),
        new((AccNationality)52, "Cuba", "CUB"),
        new((AccNationality)53, "Czech Republic", "CZE"),
        new((AccNationality)54, "Estonia", "EST"),
        new((AccNationality)55, "Georgia", "GEO"),
        new((AccNationality)56, "India", "IND"),
        new((AccNationality)57, "Israel", "ISR"),
        new((AccNationality)58, "Jamaica", "JAM"),
        new((AccNationality)59, "Latvia", "LVA"),
        new((AccNationality)60, "Lithuania", "LTU"),
        new((AccNationality)61, "Macau", "MAC"),
        new((AccNationality)62, "Malaysia", "MYS"),
        new((AccNationality)63, "Nepal", "NPL"),
        new((AccNationality)64, "New Caledonia", "NCL"),
        new((AccNationality)65, "Nigeria", "NER"),
        new((AccNationality)66, "Northern Ireland", "NIR"),
        new((AccNationality)67, "Papua New Guinea", "PNG"),
        new((AccNationality)68, "Philippines", "PHL"),
        new((AccNationality)69, "Qatar", "QAT"),
        new((AccNationality)70, "Romania", "ROU"),
        new((AccNationality)71, "Scotland", "GBR-SCT"),
        new((AccNationality)72, "Serbia", "SRB"),
        new((AccNationality)73, "Slovenia", "SVK"),
        new((AccNationality)74, "Taiwan", "TWN"),
        new((AccNationality)75, "Ukraine", "UKR"),
        new((AccNationality)76, "Venezuela", "VEN"),
        new((AccNationality)77, "Wales", "GBR-CYM"),
        new((AccNationality)78, "Iran", "IRN"),
        new((AccNationality)79, "Bahrain", "BHR"),
        new((AccNationality)80, "Zimbabwe", "ZWE"),
        new((AccNationality)81, "Chinese Taipei", "CHN"),
        new((AccNationality)82, "Chile", "CHL"),
        new((AccNationality)83, "Uruguay", "URU"),
        new((AccNationality)84, "Madagascar", "MAD"),
        new((AccNationality)86, "England", "GBR-ENG")
    ];

    public static AccNationalityInfoProvider Instance =>
        singletonInstance ??= new AccNationalityInfoProvider();

    public AccNationalityInfo? FindById(AccNationality accNationality)
    {
        return this.Nationalities.FirstOrDefault(n => n.AccNationality == accNationality);
    }

    public string GetCountryCode(AccNationality accNationality)
    {
        return this.FindById(accNationality)
                   ?.CountryCode ?? "ZZZ";
    }
}