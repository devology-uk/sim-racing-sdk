using SimRacingSdk.GameControl.Models;

namespace SimRacingSdk.GameControl.Abstractions;

public interface IGameKeyboard
{
    Task<GameKeyPressResult> PressAsync(GameWindow gameWindow, GameKeyPress keyPress);
}
