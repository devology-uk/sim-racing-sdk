using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.Pmr.DataManager.Cars;
using SimRacingSdk.Pmr.DataManager.Session;
using SimRacingSdk.Pmr.DataManager.SetupMaps;
using SimRacingSdk.Pmr.DataManager.Tracks;

namespace SimRacingSdk.Pmr.DataManager;

public partial class App : DataManagerApplication
{
    protected override DataManagerOptions Options { get; } = new()
    {
        GameCode = "Pmr",
        GameName = "Project Motor Racing"
    };

    protected override void ConfigureGameServices(IServiceCollection services)
    {
        services.AddSingleton<SessionStateStore<PmrSessionState>>();
        services.AddSingleton<ISessionStateStore<PmrSessionState>>(provider =>
            provider.GetRequiredService<SessionStateStore<PmrSessionState>>());
        services.AddSingleton<ISessionStateStore>(provider =>
            provider.GetRequiredService<SessionStateStore<PmrSessionState>>());

        services.UseCatalogRepository<CarInfo>("cars");
        services.AddSingleton<ICatalogGenerator<CarInfo>, PmrCarProviderGenerator>();
        services.UseCatalogRepository<TrackInfo>("tracks");
        services.AddSingleton<ICatalogGenerator<TrackInfo>, PmrTrackProviderGenerator>();
        services.AddSingleton<ISetupMapRepository, SetupMapRepository>();
        services.AddSingleton<IDefaultSetupFieldApplier, DefaultSetupFieldApplier>();
        services.AddSingleton<IPmrSetupMapsGenerator, PmrSetupMapsGenerator>();

        services.UsePage<CarsViewModel>("Cars");
        services.UsePage<TracksViewModel>("Tracks");
        services.UsePage<SetupMapsViewModel>("Setup Maps");
        services.AddSingleton<IUnsavedChangesSaver>(provider => provider.GetRequiredService<SetupMapsViewModel>());
    }
}
