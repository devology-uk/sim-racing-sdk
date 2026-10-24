using SimRacingSdk.GameControl.Abstractions;
using SimRacingSdk.Lmu.Core.Services;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

public class LmuWindowStateReader
{
    private readonly IGameWindowFinder gameWindowFinder;
    private readonly IGameWindowStateReader gameWindowStateReader;

    public LmuWindowStateReader(IGameWindowFinder gameWindowFinder, IGameWindowStateReader gameWindowStateReader)
    {
        this.gameWindowFinder = gameWindowFinder;
        this.gameWindowStateReader = gameWindowStateReader;
    }

    public string Read()
    {
        var gameWindow = this.gameWindowFinder.Find(LmuGameDetector.ProcessName);
        return gameWindow == null ? "not found" : this.gameWindowStateReader.Read(gameWindow).ToString();
    }
}
