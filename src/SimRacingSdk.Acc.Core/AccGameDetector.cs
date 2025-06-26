using System.Diagnostics;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Core.Services;

namespace SimRacingSdk.Acc.Core
{
    public class AccGameDetector : GameDetector, IAccGameDetector
    {
        private const string ProcessName = "AC2-Win64-Shipping";
        
        private AccGameDetector? singletonInstance;

        public AccGameDetector Instance => this.singletonInstance ??= new AccGameDetector();

        protected override bool IsGameRunning()
        {
            return Process.GetProcessesByName(ProcessName)
                          .Any();
        }
    }
}