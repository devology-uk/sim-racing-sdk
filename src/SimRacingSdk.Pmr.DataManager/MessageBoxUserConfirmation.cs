using System.Windows;

namespace SimRacingSdk.Pmr.DataManager;

public class MessageBoxUserConfirmation : IUserConfirmation
{
    public bool Confirm(string question, string title)
    {
        var answer = MessageBox.Show(Application.Current.MainWindow!,
            question,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question,
            MessageBoxResult.No);
        return answer == MessageBoxResult.Yes;
    }
}
