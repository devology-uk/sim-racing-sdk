using SimRacingSdk.GameControl.Models;

namespace SimRacingSdk.GameControl.Abstractions;

public interface IKeystrokeSender
{
    Task<bool> PressAsync(GameKeyPress keyPress);
}
