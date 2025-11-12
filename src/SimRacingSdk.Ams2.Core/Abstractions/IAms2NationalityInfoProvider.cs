using SimRacingSdk.Ams2.Core.Enums;
using SimRacingSdk.Ams2.Core.Models;

namespace SimRacingSdk.Ams2.Core.Abstractions;

public interface IAms2NationalityInfoProvider
{
    Ams2NationalityInfo? FindById(Ams2Nationality accNationality);
    string GetCountryCode(Ams2Nationality accNationality);
}