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

    public static string ToFriendlyName(this Nationality nationality)
    {
        return nationality switch
        {
            Nationality.ChineseTaipei => "Chinese Taipei",
            Nationality.CzechRepublic => "Czech Republic",
            Nationality.GreatBritain => "Great Britain",
            Nationality.HongKong => "Hong Kong",
            Nationality.NewCaledonia => "New Caledonia",
            Nationality.NewZealand => "New Zealand",
            Nationality.NorthernIreland => "Northern Ireland",
            Nationality.PapuaNewGuinea => "Papua New Guinea",
            Nationality.PuertoRico => "Puerto Rico",
            Nationality.SanMarino => "San Marino",
            Nationality.SaudiArabia => "Saudi Arabia",
            Nationality.SouthAfrica => "South Africa",
            Nationality.SouthKorea => "South Korea",
            _ => nationality.ToString()
        };
    }
}