namespace SimRacingSdk.R3e.DataManager.GameData;

public record ImportResult
{
    public int Added { get; init; }

    // Captured entities whose game id the game no longer lists. Kept, never deleted automatically.
    public int NotInGame { get; init; }

    public int Unchanged { get; init; }
    public int Updated { get; init; }

    public string Describe(string entityName)
    {
        return $"Imported {entityName} at {DateTime.Now:HH:mm:ss}: {this.Added} added, {this.Updated} updated, "
               + $"{this.Unchanged} unchanged, {this.NotInGame} no longer in the game.";
    }
}
