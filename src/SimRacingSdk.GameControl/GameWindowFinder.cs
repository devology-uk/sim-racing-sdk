using System.Diagnostics;
using SimRacingSdk.GameControl.Abstractions;
using SimRacingSdk.GameControl.Models;

namespace SimRacingSdk.GameControl;

public class GameWindowFinder : IGameWindowFinder
{
    public GameWindow? Find(string processName)
    {
        var processes = Process.GetProcessesByName(processName);
        try
        {
            var windowHandle = processes.Select(process => process.MainWindowHandle)
                                        .FirstOrDefault(handle => handle != IntPtr.Zero);
            return windowHandle == IntPtr.Zero ? null : new GameWindow(processName, windowHandle);
        }
        finally
        {
            foreach(var process in processes)
            {
                process.Dispose();
            }
        }
    }
}
