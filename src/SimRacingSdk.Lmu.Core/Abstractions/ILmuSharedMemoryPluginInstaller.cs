namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuSharedMemoryPluginInstaller
{
    bool IsPluginConfigured { get; }
    bool IsPluginFileInstalled { get; }
    bool IsInstalled { get; }
    void Install();
}
