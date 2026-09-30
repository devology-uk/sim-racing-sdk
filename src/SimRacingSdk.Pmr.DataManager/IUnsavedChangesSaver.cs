namespace SimRacingSdk.Pmr.DataManager;

public interface IUnsavedChangesSaver
{
    // Says what couldn't be saved when SaveUnsavedChanges returns false.
    string UnsavedChangesDescription { get; }

    bool SaveUnsavedChanges();
}
