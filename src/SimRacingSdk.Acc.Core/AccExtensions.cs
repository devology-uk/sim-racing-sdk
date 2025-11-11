using SimRacingSdk.Acc.Core.Enums;

namespace SimRacingSdk.Acc.Core;

public static class AccExtensions
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

    public static string ToFriendlyName(this AccNationality accNationality)
    {
        return accNationality switch
        {
            AccNationality.ChineseTaipei => "Chinese Taipei",
            AccNationality.CzechRepublic => "Czech Republic",
            AccNationality.GreatBritain => "Great Britain",
            AccNationality.HongKong => "Hong Kong",
            AccNationality.NewCaledonia => "New Caledonia",
            AccNationality.NewZealand => "New Zealand",
            AccNationality.NorthernIreland => "Northern Ireland",
            AccNationality.PapuaNewGuinea => "Papua New Guinea",
            AccNationality.PuertoRico => "Puerto Rico",
            AccNationality.SanMarino => "San Marino",
            AccNationality.SaudiArabia => "Saudi Arabia",
            AccNationality.SouthAfrica => "South Africa",
            AccNationality.SouthKorea => "South Korea",
            _ => accNationality.ToString()
        };
    }
}