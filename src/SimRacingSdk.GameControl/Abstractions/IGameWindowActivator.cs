using SimRacingSdk.GameControl.Models;

namespace SimRacingSdk.GameControl.Abstractions;

public interface IGameWindowActivator
{
    bool IsActive(GameWindow gameWindow);
    Task<bool> ActivateAsync(GameWindow gameWindow);
}
