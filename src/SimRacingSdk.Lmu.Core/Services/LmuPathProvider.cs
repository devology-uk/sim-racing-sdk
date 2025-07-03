using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Services
{
    public class LmuPathProvider : ILmuPathProvider
    {
        private const string GameName = "Le Mans Ultimate";

        private static readonly LmuPathProvider? singletonInstance;

        private readonly string gamePath;

        public LmuPathProvider(ISteamInfoProvider steamInfoProvider)
        {
            this.gamePath = steamInfoProvider.GetGamePath(GameName);
        }

        public static LmuPathProvider Instance { get; } =
            singletonInstance ??= new LmuPathProvider(SteamInfoProvider.Instance);

        public string LogFolder => Path.Combine(this.UserDataFolder, "Log");
        public string PlayerFolder => Path.Combine(this.UserDataFolder, "player");
        public string ResultsFolder => Path.Combine(this.LogFolder, "Results");
        public string SettingsFilePath => Path.Combine(this.PlayerFolder, "Settings.JSON");
        public string UserDataFolder => Path.Combine(this.gamePath, "UserData");
    }
}