using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// Backs one tab's worth of fields - a thin editable list wrapper reused across all five tabs so
// each one gets identical Add/Remove behaviour from a single SetupFieldsView UserControl.
public partial class SetupFieldListViewModel : ObservableObject
{
    [ObservableProperty]
    private SetupFieldEditorViewModel? selectedField;

    public ObservableCollection<SetupFieldEditorViewModel> Fields { get; } = [];

    public void LoadFrom(IEnumerable<SetupFieldInfo> fields)
    {
        this.Fields.Clear();
        foreach(var field in fields)
        {
            this.Fields.Add(SetupFieldEditorViewModel.From(field));
        }

        this.SelectedField = this.Fields.FirstOrDefault();
    }

    public List<SetupFieldInfo> ToSetupFieldInfos()
    {
        return this.Fields.Select(field => field.ToSetupFieldInfo()).ToList();
    }

    [RelayCommand]
    private void AddField()
    {
        var field = new SetupFieldEditorViewModel();
        this.Fields.Add(field);
        this.SelectedField = field;
    }

    [RelayCommand]
    private void RemoveField()
    {
        if(this.SelectedField is null)
        {
            return;
        }

        this.Fields.Remove(this.SelectedField);
        this.SelectedField = this.Fields.FirstOrDefault();
    }
}
