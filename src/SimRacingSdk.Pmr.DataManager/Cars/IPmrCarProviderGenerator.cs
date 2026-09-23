namespace SimRacingSdk.Pmr.DataManager.Cars;

public interface IPmrCarProviderGenerator
{
    // Writes SimRacingSdk.Pmr.Core's PmrCarInfoProvider.cs, Models/PmrCarInfo.cs and
    // Abstractions/IPmrCarInfoProvider.cs from the given cars - the DataManager owns Core's whole
    // car-catalog surface, not just the provider's data, so nothing there has to pre-exist.
    // Returns the path written to for PmrCarInfoProvider.cs.
    string Generate(IEnumerable<CarInfo> cars);
}
