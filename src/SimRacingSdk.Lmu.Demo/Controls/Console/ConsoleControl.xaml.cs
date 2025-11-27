using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace SimRacingSdk.Lmu.Demo.Controls.Console;

public partial class ConsoleControl : UserControl
{
    private bool autoScroll = true;

    public ConsoleControl()
    {
        this.InitializeComponent();
        this.DataContext = App.Current.Services.GetRequiredService<ConsoleControlViewModel>();
    }

    private void HandleScrollChanged(object sender, ScrollChangedEventArgs eventArgs)
    {
        if(eventArgs.ExtentHeightChange == 0)
        {
            this.autoScroll = this.ScrollViewer.VerticalOffset <= this.ScrollViewer.ScrollableHeight;
        }

        if(this.autoScroll && eventArgs.ExtentHeightChange != 0)
        {
            this.ScrollViewer.ScrollToVerticalOffset(this.ScrollViewer.ExtentHeight);
        }
    }
}