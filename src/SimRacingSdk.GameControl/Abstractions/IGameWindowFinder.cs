using SimRacingSdk.GameControl.Models;

namespace SimRacingSdk.GameControl.Abstractions;

public interface IGameWindowFinder
{
    GameWindow? Find(string processName);
}
