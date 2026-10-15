namespace SimRacingSdk.DataManager.Shared;

public interface IUserConfirmation
{
    bool Confirm(string question, string title);
}
