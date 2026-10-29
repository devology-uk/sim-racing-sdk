using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.DataManager.Shared.Catalog;

// Browse, edit, save and generate for one catalog entity, mirroring a game's own select screens:
// "By <group>" (a class, a continent...) or everything at once.
public abstract partial class CatalogPageViewModel<TEntity, TEditor> : ObservableObject
    where TEntity : class, ICatalogEntity
    where TEditor : ICatalogEditor<TEntity>
{
    private readonly List<TEntity> entities;
    private readonly ICatalogGenerator<TEntity> generator;
    private readonly string pageKey;
    private readonly IEntityRepository<TEntity> repository;
    private readonly ISessionStateStore sessionStateStore;

    [ObservableProperty]
    private CatalogBrowseMode browseMode = CatalogBrowseMode.ByGroup;

    [ObservableProperty]
    private string generateStatusMessage = string.Empty;

    [ObservableProperty]
    private string saveStatusMessage = string.Empty;

    [ObservableProperty]
    private string? selectedGroup;

    [ObservableProperty]
    private TEntity? selectedItem;

    protected CatalogPageViewModel(
        string pageKey,
        TEditor editor,
        IEntityRepository<TEntity> repository,
        ICatalogGenerator<TEntity> generator,
        ISessionStateStore sessionStateStore)
    {
        this.pageKey = pageKey;
        this.Editor = editor;
        this.repository = repository;
        this.generator = generator;
        this.sessionStateStore = sessionStateStore;
        this.entities = repository.GetAll().ToList();
    }

    public TEditor Editor { get; }

    // Not a fixed list - derived from the saved entities, so a new DLC class or region shows up the
    // moment an entity using it is saved, with no code change needed.
    public ObservableCollection<string> Groups { get; } = [];

    public bool IsByGroupMode => this.BrowseMode == CatalogBrowseMode.ByGroup;
    public ObservableCollection<TEntity> Items { get; } = [];

    protected IReadOnlyList<TEntity> Entities => this.entities;

    private CatalogPageState PageState => this.sessionStateStore.State.CatalogPage(this.pageKey);

    // Called last by the derived constructor, so overrides can rely on its own fields.
    protected void Initialise()
    {
        this.RefreshGroups();
        this.OnEntitiesChanged();
        this.RestoreSession();
    }

    // For entities saved to the repository by something other than this page, e.g. an import from the game's own data.
    protected void Reload()
    {
        var selectedId = this.SelectedItem?.Id;
        this.entities.Clear();
        this.entities.AddRange(this.repository.GetAll());

        this.OnEntitiesChanged();
        this.RefreshShownEntities(selectedId);
    }

    // For a change to what IsShown decides, e.g. a filter checkbox.
    protected void RefreshShownEntities()
    {
        this.RefreshShownEntities(this.SelectedItem?.Id);
    }

    protected abstract string GroupOf(TEntity entity);
    protected abstract IEnumerable<TEntity> Sort(IEnumerable<TEntity> unsorted);

    // Appended to the Generate status, e.g. " - 3 still missing a ModelId".
    protected virtual string DescribeGaps()
    {
        return string.Empty;
    }

    // E.g. to refresh the suggestion lists behind editable combo boxes.
    protected virtual void OnEntitiesChanged()
    {
    }

    protected virtual void OnItemRenamed(string originalId, string newId)
    {
    }

    // E.g. to hide entities the player can't pick in the game.
    protected virtual bool IsShown(TEntity entity)
    {
        return true;
    }

    protected virtual string? ValidationProblem(TEntity entity)
    {
        return string.IsNullOrWhiteSpace(this.GroupOf(entity)) ? "its group is empty" : null;
    }

    [RelayCommand]
    private void GenerateCoreProvider()
    {
        var path = this.generator.Generate(this.entities);
        this.GenerateStatusMessage =
            $"Generated {this.entities.Count} {this.pageKey.ToLowerInvariant()} to {path} at {DateTime.Now:HH:mm:ss}"
            + $"{this.DescribeGaps()}.";
    }

    [RelayCommand]
    private void New()
    {
        this.SelectedItem = null;
        this.Editor.LoadFrom(null);
        if(this.IsByGroupMode && this.SelectedGroup is not null)
        {
            this.Editor.StartInGroup(this.SelectedGroup);
        }
    }

    [RelayCommand]
    private void Save()
    {
        var entity = this.Editor.ToEntity();
        var problem = this.ValidationProblem(entity);
        if(problem is not null)
        {
            this.SaveStatusMessage = $"Not saved - {problem}.";
            return;
        }

        var originalId = this.SelectedItem?.Id;
        this.repository.Save(entity);

        if(originalId is not null && originalId != entity.Id)
        {
            this.RemoveRenamedItem(originalId, entity.Id);
        }

        this.entities.RemoveAll(existing => existing.Id == entity.Id);
        this.entities.Add(entity);

        this.RefreshGroups();
        this.OnEntitiesChanged();
        this.SelectedGroup = this.GroupOf(entity);
        this.RefreshItems();
        this.SelectedItem = this.Items.FirstOrDefault(item => item.Id == entity.Id);
        this.SaveStatusMessage = $"Saved {entity.DisplayName} at {DateTime.Now:HH:mm:ss}.";
    }

    [RelayCommand]
    private void SetBrowseMode(CatalogBrowseMode mode)
    {
        this.BrowseMode = mode;
    }

    partial void OnBrowseModeChanged(CatalogBrowseMode value)
    {
        this.OnPropertyChanged(nameof(this.IsByGroupMode));
        this.RefreshItems();
        this.PageState.BrowseMode = value.ToString();
        this.sessionStateStore.Save();
    }

    partial void OnSelectedGroupChanged(string? value)
    {
        this.RefreshItems();
        this.PageState.Group = value;
        this.sessionStateStore.Save();
    }

    partial void OnSelectedItemChanged(TEntity? value)
    {
        this.Editor.LoadFrom(value);
        this.PageState.ItemId = value?.Id;
        this.sessionStateStore.Save();
    }

    // Reads every saved value up front - applying each one fires change handlers that overwrite the
    // rest of the saved state (e.g. picking a group selects its first item).
    private void RestoreSession()
    {
        var savedBrowseMode = this.PageState.BrowseMode;
        var savedGroup = this.PageState.Group;
        var savedItemId = this.PageState.ItemId;

        if(Enum.TryParse<CatalogBrowseMode>(savedBrowseMode, out var restoredBrowseMode))
        {
            this.BrowseMode = restoredBrowseMode;
        }

        this.SelectedGroup = this.Groups.Contains(savedGroup ?? string.Empty) ? savedGroup : this.Groups.FirstOrDefault();
        if(this.Items.Count == 0)
        {
            this.RefreshItems();
        }

        this.SelectedItem = this.Items.FirstOrDefault(item => item.Id == savedItemId) ?? this.SelectedItem;
    }

    // The Id derives from the entity's identifying fields, so editing any of them moves it to a new file.
    private void RemoveRenamedItem(string originalId, string newId)
    {
        this.repository.Delete(originalId);
        this.entities.RemoveAll(existing => existing.Id == originalId);
        this.OnItemRenamed(originalId, newId);
    }

    private void RefreshShownEntities(string? selectedId)
    {
        this.RefreshGroups();
        this.SelectedGroup = this.Groups.Contains(this.SelectedGroup ?? string.Empty)
            ? this.SelectedGroup
            : this.Groups.FirstOrDefault();
        this.RefreshItems();
        this.SelectedItem = this.Items.FirstOrDefault(item => item.Id == selectedId) ?? this.SelectedItem;
    }

    private void RefreshGroups()
    {
        var distinctGroups = this.entities.Where(this.IsShown).Select(this.GroupOf).Distinct().OrderBy(group => group);

        this.Groups.Clear();
        foreach(var group in distinctGroups)
        {
            this.Groups.Add(group);
        }
    }

    private void RefreshItems()
    {
        this.Items.Clear();

        var shownEntities = this.entities.Where(this.IsShown);
        var itemsToShow = this.IsByGroupMode
            ? shownEntities.Where(entity => this.GroupOf(entity) == this.SelectedGroup)
            : shownEntities;

        foreach(var item in this.Sort(itemsToShow))
        {
            this.Items.Add(item);
        }

        this.SelectedItem = this.Items.FirstOrDefault();
    }
}
