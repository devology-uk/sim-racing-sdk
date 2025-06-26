using System.Diagnostics;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core
{
    public class LmuGameDetector : GameDetector, ILmuGameDetector
    {
        private const string ProcessName = "Le Mans Ultimate"; //TODO: Update to the correct process name for LMU0
        
        private static LmuGameDetector? singletonInstance;

        public static LmuGameDetector Instance => singletonInstance ??= new LmuGameDetector();

        protected override bool IsGameRunning()
        {
            return Process.GetProcessesByName(ProcessName)
                          .Any();
        }
    }
}