using SimRacingSdk.Ace.Core.Enums;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAceNationalityInfoProvider
{
    AceNationalityInfo? FindById(AceNationality aceNationality);
    string GetCountryCode(AceNationality aceNationality);
}
