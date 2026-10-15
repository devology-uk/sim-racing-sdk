namespace SimRacingSdk.DataManager.Shared.Storage;

public interface ICatalogEntity
{
    string DisplayName { get; }

    // The JSON file's name - a slug of whichever fields make the entity unique.
    string Id { get; }
}
