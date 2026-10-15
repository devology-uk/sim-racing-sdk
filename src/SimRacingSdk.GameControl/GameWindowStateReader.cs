using SimRacingSdk.GameControl.Abstractions;
using SimRacingSdk.GameControl.Models;
using SimRacingSdk.GameControl.Native;

namespace SimRacingSdk.GameControl;

public class GameWindowStateReader : IGameWindowStateReader
{
    public GameWindowState Read(GameWindow gameWindow)
    {
        if(NativeMethods.IsIconic(gameWindow.Handle))
        {
            return GameWindowState.Minimised;
        }

        return NativeMethods.GetForegroundWindow() == gameWindow.Handle
            ? GameWindowState.Active
            : GameWindowState.Background;
    }
}
