using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

public partial class RouteValueInput : ObservableObject
{
    [ObservableProperty]
    private string value = string.Empty;

    public RouteValueInput(string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}
