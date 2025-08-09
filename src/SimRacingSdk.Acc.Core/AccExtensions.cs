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
}