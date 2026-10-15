using SimRacingSdk.GameControl.Models;

namespace SimRacingSdk.GameControl.Abstractions;

public interface IGameWindowStateReader
{
    GameWindowState Read(GameWindow gameWindow);
}
