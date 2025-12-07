using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.LogViewer;

public partial class FilterItem : ObservableObject
{
    [ObservableProperty]
    private bool isSelected;
    [ObservableProperty]
    private string name;

    public FilterItem(string name)
    {
        this.Name = name;
        this.IsSelected = true;
    }
}