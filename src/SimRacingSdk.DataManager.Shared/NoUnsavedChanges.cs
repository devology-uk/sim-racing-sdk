namespace SimRacingSdk.DataManager.Shared;

// For a Data Manager whose pages all save explicitly, so there's never anything pending on close.
public class NoUnsavedChanges : IUnsavedChangesSaver
{
    public string UnsavedChangesDescription => string.Empty;

    public bool SaveUnsavedChanges()
    {
        return true;
    }
}
