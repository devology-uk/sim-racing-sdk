namespace SimRacingSdk.R3e.DataManager.GameData;

// Brings the captured catalog in line with the game's own data: adds new content and refreshes
// game-owned fields (ids, names, classes) while keeping everything curated by hand.
public interface ICatalogImporter<TEntity>
{
    ImportResult Import();
}
