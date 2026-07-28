using SimRacingSdk.Ace.Core.Enums;

namespace SimRacingSdk.Ace.Core;

public static class AceExtensions
{
    public static string ToFriendlyName(this RaceSessionType sessionType)
    {
        return sessionType switch
               {
                   RaceSessionType.HotlapSuperpole => "Hotlap Superpole",
                   _ => sessionType.ToString()
               };
    }

    public static string ToFriendlyName(this SessionPhase sessionType)
    {
        return sessionType switch
               {
                   SessionPhase.FormationLap => "Formation Lap",
                   SessionPhase.PostSession => "Post Session",
                   SessionPhase.PreFormation => "Pre Formation",
                   SessionPhase.PreSession => "Pre Session",
                   SessionPhase.SessionOver => "Session Over",
                   _ => sessionType.ToString()
               };
    }

    public static string ToFriendlyName(this AceNationality aceNationality)
    {
        return aceNationality switch
        {
            AceNationality.ChineseTaipei => "Chinese Taipei",
            AceNationality.CzechRepublic => "Czech Republic",
            AceNationality.GreatBritain => "Great Britain",
            AceNationality.HongKong => "Hong Kong",
            AceNationality.NewCaledonia => "New Caledonia",
            AceNationality.NewZealand => "New Zealand",
            AceNationality.NorthernIreland => "Northern Ireland",
            AceNationality.PapuaNewGuinea => "Papua New Guinea",
            AceNationality.PuertoRico => "Puerto Rico",
            AceNationality.SanMarino => "San Marino",
            AceNationality.SaudiArabia => "Saudi Arabia",
            AceNationality.SouthAfrica => "South Africa",
            AceNationality.SouthKorea => "South Korea",
            _ => aceNationality.ToString()
        };
    }
}
