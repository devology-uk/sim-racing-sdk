using SimRacingSdk.Pmr.Setups.Models.Files;
using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Abstractions;

public interface IPmrSetupPresenter
{
    // Both setups must be for the same car.
    PmrSetupComparison Compare(PmrSetupFile first, PmrSetupFile second, PmrUnitSystem unitSystem);

    PmrSetupView Present(PmrSetupFile setup, PmrUnitSystem unitSystem);
}
