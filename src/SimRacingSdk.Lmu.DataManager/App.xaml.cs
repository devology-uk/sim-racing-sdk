using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.Lmu.DataManager.Cars;
using SimRacingSdk.Lmu.DataManager.Tracks;

namespace SimRacingSdk.Lmu.DataManager;

public partial class App : DataManagerApplication
{
    protected override DataManagerOptions Options { get; } = new()
    {
        GameCode = "Lmu",
        GameName = "Le Mans Ultimate"
    };

    protected override void ConfigureGameServices(IServiceCollection services)
    {
        services.UseCatalogRepository<CarInfo>("cars");
        services.AddSingleton<ICatalogGenerator<CarInfo>, LmuCarProviderGenerator>();
        services.UseCatalogRepository<TrackInfo>("tracks");
        services.AddSingleton<ICatalogGenerator<TrackInfo>, LmuTrackProviderGenerator>();

        services.UsePage<CarsViewModel>("Cars");
        services.UsePage<TracksViewModel>("Tracks");
    }
}
