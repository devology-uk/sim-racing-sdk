using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuGameDataProvider
{
    LmuSettings GetSettings();
    IList<string> ListResultFiles();
}