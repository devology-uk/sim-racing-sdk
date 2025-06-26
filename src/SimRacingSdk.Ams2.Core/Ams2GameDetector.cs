using System.Diagnostics;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Core.Services;

namespace SimRacingSdk.Ams2.Core
{
    public class Ams2GameDetector : GameDetector, IAms2GameDetector
    {
        private readonly string[] processNames = ["AMS2", "AMS2AVX"];

        private Ams2GameDetector? singletonInstance;

        public Ams2GameDetector Instance => this.singletonInstance ??= new Ams2GameDetector();

        protected override bool IsGameRunning()
        {
            return this.processNames.Any(processName => Process.GetProcessesByName(processName)
                                                               .Length > 0);
        }
    }
}