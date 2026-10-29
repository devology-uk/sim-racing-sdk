using System.IO;
using System.Text.Json;
using SimRacingSdk.R3e.Core.Abstractions;

namespace SimRacingSdk.R3e.DataManager.GameData;

// Reads r3e-data.json from the player's own install, so it always matches their game version.
// Each section is an object keyed by id; cars refer to their class and manufacturer by id.
public class GameCatalogReader : IGameCatalogReader
{
    private readonly IR3ePathProvider r3ePathProvider;

    public GameCatalogReader(IR3ePathProvider r3ePathProvider)
    {
        this.r3ePathProvider = r3ePathProvider;
    }

    public GameCatalog Read()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(this.r3ePathProvider.CatalogFilePath));
        var root = document.RootElement;
        var classNames = this.ReadNamesById(root.GetProperty("classes"));
        var manufacturerNames = this.ReadNamesById(root.GetProperty("manufacturers"));

        return new GameCatalog
        {
            Cars = this.Entries(root.GetProperty("cars"))
                       .Select(car => this.ReadCar(car, classNames, manufacturerNames))
                       .ToList(),
            Tracks = this.Entries(root.GetProperty("tracks"))
                         .Select(this.ReadTrack)
                         .ToList()
        };
    }

    private GameCar ReadCar(JsonElement car,
        IReadOnlyDictionary<int, string> classNames,
        IReadOnlyDictionary<int, string> manufacturerNames)
    {
        return new GameCar
        {
            Class = classNames[car.GetProperty("Class").GetInt32()],
            GameId = car.GetProperty("Id").GetInt32(),
            Manufacturer = manufacturerNames[car.GetProperty("CarManufacturer").GetInt32()],
            Name = this.ReadName(car)
        };
    }

    private GameTrack ReadTrack(JsonElement track)
    {
        return new GameTrack
        {
            GameId = track.GetProperty("Id").GetInt32(),
            Layouts = track.GetProperty("layouts")
                           .EnumerateArray()
                           .Select(this.ReadLayout)
                           .ToList(),
            Name = this.ReadName(track)
        };
    }

    private GameLayout ReadLayout(JsonElement layout)
    {
        return new GameLayout
        {
            GameId = layout.GetProperty("Id").GetInt32(),
            GridSize = layout.GetProperty("MaxNumberOfVehicles").GetInt32(),
            Name = this.ReadName(layout)
        };
    }

    private IReadOnlyDictionary<int, string> ReadNamesById(JsonElement section)
    {
        return this.Entries(section)
                   .ToDictionary(entry => entry.GetProperty("Id").GetInt32(), this.ReadName);
    }

    private IEnumerable<JsonElement> Entries(JsonElement section)
    {
        return section.EnumerateObject()
                      .Select(property => property.Value);
    }

    private string ReadName(JsonElement element)
    {
        return element.GetProperty("Name").GetString()?.Trim() ?? string.Empty;
    }
}
