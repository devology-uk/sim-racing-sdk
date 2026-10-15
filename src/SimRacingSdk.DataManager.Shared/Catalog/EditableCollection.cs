using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimRacingSdk.DataManager.Shared.Catalog;

// A nested list inside an entity editor (e.g. a track's layouts) with add/remove and a selection
// for the detail form beside it.
public partial class EditableCollection<TItem> : ObservableObject where TItem : class, new()
{
    [ObservableProperty]
    private TItem? selectedItem;

    public ObservableCollection<TItem> Items { get; } = [];

    public void Replace(IEnumerable<TItem> items)
    {
        this.Items.Clear();
        foreach(var item in items)
        {
            this.Items.Add(item);
        }

        this.SelectedItem = this.Items.FirstOrDefault();
    }

    [RelayCommand]
    private void Add()
    {
        var item = new TItem();
        this.Items.Add(item);
        this.SelectedItem = item;
    }

    [RelayCommand]
    private void Remove()
    {
        if(this.SelectedItem is null)
        {
            return;
        }

        this.Items.Remove(this.SelectedItem);
        this.SelectedItem = this.Items.FirstOrDefault();
    }
}
