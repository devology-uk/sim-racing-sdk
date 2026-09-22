namespace SimRacingSdk.Pmr.DataManager.Cars;

public interface IPmrCarProviderGenerator
{
    // Writes SimRacingSdk.Pmr.Core/PmrCarInfoProvider.cs from the given cars and returns the path
    // written to.
    string Generate(IEnumerable<CarInfo> cars);
}
