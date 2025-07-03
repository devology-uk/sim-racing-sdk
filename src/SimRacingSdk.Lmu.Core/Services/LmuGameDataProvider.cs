using System.Text.Json;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Services
{
    public class LmuGameDataProvider : ILmuGameDataProvider
    {
        private static LmuGameDataProvider? singletonInstance;
        private readonly ILmuPathProvider pathProvider;

        public LmuGameDataProvider(ILmuPathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        public static LmuGameDataProvider Instance =>
            singletonInstance ??= new LmuGameDataProvider(LmuPathProvider.Instance);

        public LmuSettings GetSettings()
        {
            if(!File.Exists(this.pathProvider.SettingsFilePath))
            {
                return new LmuSettings();
            }

            var json = File.ReadAllText(this.pathProvider.SettingsFilePath);
            return JsonSerializer.Deserialize<LmuSettings>(json)!;
        }

        public IList<string> ListResultFiles()
        {
            return Directory.GetFiles(this.pathProvider.ResultsFolder, "*.xml").ToList();
        }
    }
}