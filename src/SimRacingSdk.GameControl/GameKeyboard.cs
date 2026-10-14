using SimRacingSdk.GameControl.Abstractions;
using SimRacingSdk.GameControl.Models;

namespace SimRacingSdk.GameControl;

// SendInput types into whatever window has focus, so a press is refused unless the game is the active window.
public class GameKeyboard : IGameKeyboard
{
    private readonly IGameWindowActivator gameWindowActivator;
    private readonly IKeystrokeSender keystrokeSender;

    public GameKeyboard(IGameWindowActivator gameWindowActivator, IKeystrokeSender keystrokeSender)
    {
        this.gameWindowActivator = gameWindowActivator;
        this.keystrokeSender = keystrokeSender;
    }

    public async Task<GameKeyPressResult> PressAsync(GameWindow gameWindow, GameKeyPress keyPress)
    {
        if(!this.gameWindowActivator.IsActive(gameWindow))
        {
            return GameKeyPressResult.GameWindowNotActive;
        }

        var isSent = await this.keystrokeSender.PressAsync(keyPress);
        return isSent ? GameKeyPressResult.Sent : GameKeyPressResult.SendFailed;
    }
}
