using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace SimRacingSdk.LogViewer;

public partial class LogViewerWindow : Window
{
    public LogViewerWindow()
    {
        this.InitializeComponent();
    }

    private void HandlePreviewKeyDown(object sender, KeyEventArgs eventArgs)
    {
        Debug.WriteLine($"Preview Key Down: Modifier: {eventArgs.KeyboardDevice.Modifiers}, Key: {eventArgs.Key}, SystemKey: {eventArgs.SystemKey}");
        if(this.DataContext is not LogViewerViewModel vm)
        {
               return;
        }

        if(eventArgs.KeyboardDevice.Modifiers != ModifierKeys.Alt
           || (eventArgs.SystemKey != Key.Left && eventArgs.SystemKey != Key.Right))
        {
            return;
        }

        eventArgs.Handled = true;

        switch(eventArgs.SystemKey)
        {
            case Key.Right:
                vm.NextPageCommand.Execute(null);
                break;
            case Key.Left:
                vm.PreviousPageCommand.Execute(null);
                break;
        }
    }
}