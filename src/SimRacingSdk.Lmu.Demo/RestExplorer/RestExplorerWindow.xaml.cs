using System.Windows;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

public partial class RestExplorerWindow : Window
{
    public RestExplorerWindow()
    {
        this.InitializeComponent();
    }

    protected override void OnClosed(EventArgs e)
    {
        (this.DataContext as IDisposable)?.Dispose();
        base.OnClosed(e);
    }
}
