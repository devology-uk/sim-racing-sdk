using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Ams2.DataManager.Cars;
using SimRacingSdk.Ams2.DataManager.Tracks;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ams2.DataManager;

public partial class App : DataManagerApplication
{
    protected override DataManagerOptions Options { get; } = new()
    {
        GameCode = "Ams2",
        GameName = "Automobilista 2"
    };

    protected override void ConfigureGameServices(IServiceCollection services)
    {
        services.UseCatalogRepository<CarInfo>("cars");
        services.AddSingleton<ICatalogGenerator<CarInfo>, Ams2CarProviderGenerator>();
        services.UseCatalogRepository<TrackInfo>("tracks");
        services.AddSingleton<ICatalogGenerator<TrackInfo>, Ams2TrackProviderGenerator>();

        services.UsePage<CarsViewModel>("Cars");
        services.UsePage<TracksViewModel>("Tracks");
    }
}
