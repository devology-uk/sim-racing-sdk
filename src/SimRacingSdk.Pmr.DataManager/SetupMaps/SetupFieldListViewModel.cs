using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// Backs one tab's worth of fields - a thin editable list wrapper reused across all five tabs so
// each one gets identical Add/Remove behaviour from a single SetupFieldsView UserControl.
public partial class SetupFieldListViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(MoveFieldUpCommand))]
    [NotifyCanExecuteChangedFor(nameof(MoveFieldDownCommand))]
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

    // Placed straight below the original, so splitting a row into Front and Rear only needs the
    // scope and ranges changed on the copy.
    [RelayCommand]
    private void DuplicateField()
    {
        if(this.SelectedField is null)
        {
            return;
        }

        var duplicate = SetupFieldEditorViewModel.From(this.SelectedField.ToSetupFieldInfo());
        this.Fields.Insert(this.SelectedFieldIndex + 1, duplicate);
        this.SelectedField = duplicate;
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

    [RelayCommand(CanExecute = nameof(CanMoveFieldUp))]
    private void MoveFieldUp()
    {
        this.MoveSelectedFieldBy(-1);
    }

    [RelayCommand(CanExecute = nameof(CanMoveFieldDown))]
    private void MoveFieldDown()
    {
        this.MoveSelectedFieldBy(1);
    }

    private bool CanMoveFieldUp()
    {
        return this.SelectedFieldIndex > 0;
    }

    private bool CanMoveFieldDown()
    {
        return this.SelectedFieldIndex >= 0 && this.SelectedFieldIndex < this.Fields.Count - 1;
    }

    private int SelectedFieldIndex => this.SelectedField is null ? -1 : this.Fields.IndexOf(this.SelectedField);

    private void MoveSelectedFieldBy(int offset)
    {
        var index = this.SelectedFieldIndex;
        this.Fields.Move(index, index + offset);
        this.MoveFieldUpCommand.NotifyCanExecuteChanged();
        this.MoveFieldDownCommand.NotifyCanExecuteChanged();
    }
}
