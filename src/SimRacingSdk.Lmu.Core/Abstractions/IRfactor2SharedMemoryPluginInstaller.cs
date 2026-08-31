namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface IRfactor2SharedMemoryPluginInstaller
{
    bool IsPluginConfigured { get; }
    bool IsPluginFileInstalled { get; }
    bool IsInstalled { get; }
    void Install();
}
