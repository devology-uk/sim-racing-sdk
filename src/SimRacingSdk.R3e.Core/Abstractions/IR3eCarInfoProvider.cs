using SimRacingSdk.R3e.Core.Models;

namespace SimRacingSdk.R3e.Core.Abstractions;

public interface IR3eCarInfoProvider
{
    R3eCarInfo? FindByGameId(int gameId);

    /// <summary>
    ///     Several cars share a name across classes and seasons, so a name alone (e.g. from a setup
    ///     file's name) can match more than one.
    /// </summary>
    IReadOnlyCollection<R3eCarInfo> FindByName(string name);

    IReadOnlyCollection<string> GetCarClasses();
    IReadOnlyCollection<R3eCarInfo> GetCarInfos();
    IReadOnlyCollection<R3eCarInfo> GetCarInfosForClass(string carClass);
}
