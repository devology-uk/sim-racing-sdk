using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.R3e.Core;
using SimRacingSdk.R3e.DataManager.Cars;
using SimRacingSdk.R3e.DataManager.GameData;
using SimRacingSdk.R3e.DataManager.Session;
using SimRacingSdk.R3e.DataManager.SetupMaps;
using SimRacingSdk.R3e.DataManager.Tracks;

namespace SimRacingSdk.R3e.DataManager;

public partial class App : DataManagerApplication
{
    protected override DataManagerOptions Options { get; } = new()
    {
        GameCode = "R3e",
        GameName = "RaceRoom Racing Experience"
    };

    protected override void ConfigureGameServices(IServiceCollection services)
    {
        services.AddSingleton<SessionStateStore<R3eSessionState>>();
        services.AddSingleton<ISessionStateStore<R3eSessionState>>(provider =>
            provider.GetRequiredService<SessionStateStore<R3eSessionState>>());
        services.AddSingleton<ISessionStateStore>(provider =>
            provider.GetRequiredService<SessionStateStore<R3eSessionState>>());

        services.UseR3eSdk();
        services.AddSingleton<IGameCatalogReader, GameCatalogReader>();

        services.UseCatalogRepository<CarInfo>("cars");
        services.AddSingleton<ICatalogGenerator<CarInfo>, R3eCarProviderGenerator>();
        services.AddSingleton<ICatalogImporter<CarInfo>, CarCatalogImporter>();
        services.UseCatalogRepository<TrackInfo>("tracks");
        services.AddSingleton<ICatalogGenerator<TrackInfo>, R3eTrackProviderGenerator>();
        services.AddSingleton<ICatalogImporter<TrackInfo>, TrackCatalogImporter>();

        services.AddSingleton<ISetupMapRepository, SetupMapRepository>();
        services.AddSingleton<IOwnedCarStore, OwnedCarStore>();
        services.AddSingleton<IR3eSetupMapsGenerator, R3eSetupMapsGenerator>();

        services.UsePage<CarsViewModel>("Cars");
        services.UsePage<TracksViewModel>("Tracks");
        services.UsePage<SetupMapsViewModel>("Setup Maps");
        services.AddSingleton<IUnsavedChangesSaver>(provider => provider.GetRequiredService<SetupMapsViewModel>());
    }
}
