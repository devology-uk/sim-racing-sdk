namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface IRfactorPluginInstaller
{
    string BundledVersion { get; }
    bool IsInstalled { get; }
    bool IsPluginConfigured { get; }
    bool IsPluginFileInstalled { get; }
    string? InstalledVersion { get; }
    void Install();
}
