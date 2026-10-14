using System.Windows;

namespace SimRacingSdk.Ace.Demo.CameraProbe;

public partial class CameraProbeWindow : Window
{
    public CameraProbeWindow()
    {
        this.InitializeComponent();
    }

    protected override void OnClosed(EventArgs e)
    {
        (this.DataContext as IDisposable)?.Dispose();
        base.OnClosed(e);
    }
}
