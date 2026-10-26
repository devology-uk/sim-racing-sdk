using SimRacingSdk.Abstractions;

namespace SimRacingSdk.Core.Services;

public class ConfigFileBackup : IConfigFileBackup
{
    private const string BackupExtension = ".bak";
    private const string TimeStampFormat = "yyyyMMdd-HHmmss";

    private static ConfigFileBackup? singletonInstance;

    public static ConfigFileBackup Instance => singletonInstance ??= new ConfigFileBackup();

    public string Backup(string filePath)
    {
        var backupFilePath = $"{filePath}.{DateTime.Now.ToString(TimeStampFormat)}{BackupExtension}";
        File.Copy(filePath, backupFilePath, true);
        return backupFilePath;
    }
}
