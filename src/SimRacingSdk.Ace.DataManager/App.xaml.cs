using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Ace.DataManager.Cars;
using SimRacingSdk.Ace.DataManager.Tracks;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ace.DataManager;

public partial class App : DataManagerApplication
{
    protected override DataManagerOptions Options { get; } = new()
    {
        GameCode = "Ace",
        GameName = "Assetto Corsa Evo"
    };

    protected override void ConfigureGameServices(IServiceCollection services)
    {
        services.UseCatalogRepository<CarInfo>("cars");
        services.AddSingleton<ICatalogGenerator<CarInfo>, AceCarProviderGenerator>();
        services.UseCatalogRepository<TrackInfo>("tracks");
        services.AddSingleton<ICatalogGenerator<TrackInfo>, AceTrackProviderGenerator>();

        services.UsePage<CarsViewModel>("Cars");
        services.UsePage<TracksViewModel>("Tracks");
    }
}
