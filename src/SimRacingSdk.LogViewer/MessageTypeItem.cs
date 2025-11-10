using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.LogViewer;

public partial class MessageTypeItem : ObservableObject
{
    [ObservableProperty]
    private bool isSelected;
    [ObservableProperty]
    private string name;

    public MessageTypeItem(string name)
    {
        this.Name = name;
        this.IsSelected = true;
    }
}