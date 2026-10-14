using SimRacingSdk.GameControl.Abstractions;
using SimRacingSdk.GameControl.Models;
using SimRacingSdk.GameControl.Native;

namespace SimRacingSdk.GameControl;

// Windows only lets the foreground process hand the foreground to another window, so activation
// has to be triggered by the user acting in this app (e.g. clicking a button), never from the background.
public class GameWindowActivator : IGameWindowActivator
{
    private static readonly TimeSpan ActivationTimeout = TimeSpan.FromMilliseconds(1000);
    private static readonly TimeSpan PollInterval = TimeSpan.FromMilliseconds(50);

    public bool IsActive(GameWindow gameWindow)
    {
        return NativeMethods.GetForegroundWindow() == gameWindow.Handle;
    }

    public async Task<bool> ActivateAsync(GameWindow gameWindow)
    {
        if(this.IsActive(gameWindow))
        {
            return true;
        }

        this.RestoreIfMinimised(gameWindow);
        NativeMethods.SetForegroundWindow(gameWindow.Handle);
        return await this.WaitUntilActive(gameWindow);
    }

    private void RestoreIfMinimised(GameWindow gameWindow)
    {
        if(NativeMethods.IsIconic(gameWindow.Handle))
        {
            NativeMethods.ShowWindow(gameWindow.Handle, NativeMethods.ShowWindowRestore);
        }
    }

    private async Task<bool> WaitUntilActive(GameWindow gameWindow)
    {
        var deadline = DateTime.UtcNow + ActivationTimeout;
        while(DateTime.UtcNow < deadline)
        {
            if(this.IsActive(gameWindow))
            {
                return true;
            }

            await Task.Delay(PollInterval);
        }

        return this.IsActive(gameWindow);
    }
}
