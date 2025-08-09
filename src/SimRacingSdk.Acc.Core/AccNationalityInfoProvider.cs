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
        new((Nationality)1, "Italy", "ITA"),
        new((Nationality)2, "Germany", "DEU"),
        new((Nationality)3, "France", "FRA"),
        new((Nationality)4, "Spain", "ESP"),
        new((Nationality)5, "Great Britain", "GBR"),
        new((Nationality)6, "Hungary", "HUN"),
        new((Nationality)7, "Belgium", "BEL"),
        new((Nationality)8, "Switzerland", "CHE"),
        new((Nationality)9, "Austria", "AUT"),
        new((Nationality)10, "Russia", "RUS"),
        new((Nationality)11, "Thailand", "THA"),
        new((Nationality)12, "Netherlands", "NLD"),
        new((Nationality)13, "Poland", "POL"),
        new((Nationality)14, "Argentina", "ARG"),
        new((Nationality)15, "Monaco", "MCO"),
        new((Nationality)16, "Ireland", "IRL"),
        new((Nationality)17, "Brazil", "BRA"),
        new((Nationality)18, "South Africa", "ZAF"),
        new((Nationality)19, "Puerto Rico", "PRI"),
        new((Nationality)20, "Slovakia", "SVK"),
        new((Nationality)21, "Oman", "OMN"),
        new((Nationality)22, "Greece", "GRC"),
        new((Nationality)23, "Saudi Arabia", "SAU"),
        new((Nationality)24, "Norway", "NOR"),
        new((Nationality)25, "Turkey", "TUR"),
        new((Nationality)26, "South Korea", "KOR"),
        new((Nationality)27, "Lebanon", "LBN"),
        new((Nationality)28, "Armenia", "ARM"),
        new((Nationality)29, "Mexico", "MEX"),
        new((Nationality)30, "Sweden", "SWE"),
        new((Nationality)31, "Finland", "FIN"),
        new((Nationality)32, "Denmark", "DNK"),
        new((Nationality)33, "Croatia", "HRV"),
        new((Nationality)34, "Canada", "CAN"),
        new((Nationality)35, "China", "CHN"),
        new((Nationality)36, "Portugal", "PRT"),
        new((Nationality)37, "Singapore", "SGP"),
        new((Nationality)38, "Indonesia", "IDN"),
        new((Nationality)39, "USA", "USA"),
        new((Nationality)40, "New Zealand", "NZL"),
        new((Nationality)41, "Australia", "AUS"),
        new((Nationality)42, "San Marino", "SMR"),
        new((Nationality)43, "UAE", "ARE"),
        new((Nationality)44, "Luxembourg", "LUX"),
        new((Nationality)45, "Kuwait", "KWT"),
        new((Nationality)46, "Hong Kong", "HKG"),
        new((Nationality)47, "Colombia", "COL"),
        new((Nationality)48, "Japan", "JPN"),
        new((Nationality)49, "Andorra", "AND"),
        new((Nationality)50, "Azerbaijan", "AZE"),
        new((Nationality)51, "Bulgaria", "BGR"),
        new((Nationality)52, "Cuba", "CUB"),
        new((Nationality)53, "Czech Republic", "CZE"),
        new((Nationality)54, "Estonia", "EST"),
        new((Nationality)55, "Georgia", "GEO"),
        new((Nationality)56, "India", "IND"),
        new((Nationality)57, "Israel", "ISR"),
        new((Nationality)58, "Jamaica", "JAM"),
        new((Nationality)59, "Latvia", "LVA"),
        new((Nationality)60, "Lithuania", "LTU"),
        new((Nationality)61, "Macau", "MAC"),
        new((Nationality)62, "Malaysia", "MYS"),
        new((Nationality)63, "Nepal", "NPL"),
        new((Nationality)64, "New Caledonia", "NCL"),
        new((Nationality)65, "Nigeria", "NER"),
        new((Nationality)66, "Northern Ireland", "NIR"),
        new((Nationality)67, "Papua New Guinea", "PNG"),
        new((Nationality)68, "Philippines", "PHL"),
        new((Nationality)69, "Qatar", "QAT"),
        new((Nationality)70, "Romania", "ROU"),
        new((Nationality)71, "Scotland", "GBR-SCT"),
        new((Nationality)72, "Serbia", "SRB"),
        new((Nationality)73, "Slovenia", "SVK"),
        new((Nationality)74, "Taiwan", "TWN"),
        new((Nationality)75, "Ukraine", "UKR"),
        new((Nationality)76, "Venezuela", "VEN"),
        new((Nationality)77, "Wales", "GBR-CYM"),
        new((Nationality)78, "Iran", "IRN"),
        new((Nationality)79, "Bahrain", "BHR"),
        new((Nationality)80, "Zimbabwe", "ZWE"),
        new((Nationality)81, "Chinese Taipei", "CHN"),
        new((Nationality)82, "Chile", "CHL"),
        new((Nationality)83, "Uruguay", "URU"),
        new((Nationality)84, "Madagascar", "MAD"),
        new((Nationality)86, "England", "GBR-ENG")
    ];

    public static AccNationalityInfoProvider Instance =>
        singletonInstance ??= new AccNationalityInfoProvider();

    public AccNationalityInfo? FindById(Nationality accNationality)
    {
        return this.Nationalities.FirstOrDefault(n => n.AccNationality == accNationality);
    }

    public string GetCountryCode(Nationality accNationality)
    {
        return this.FindById(accNationality)
                   ?.CountryCode ?? "ZZZ";
    }
}