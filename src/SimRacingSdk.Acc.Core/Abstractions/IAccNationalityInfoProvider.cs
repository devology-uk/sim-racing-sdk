using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Core.Abstractions;

public interface IAccNationalityInfoProvider
{
    AccNationalityInfo? FindById(AccNationality accNationality);
    string GetCountryCode(AccNationality accNationality);
}