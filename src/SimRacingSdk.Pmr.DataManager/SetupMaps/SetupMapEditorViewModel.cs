using System.Text.Json;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public class SetupMapEditorViewModel
{
    private string savedState = string.Empty;

    public SetupMapEditorViewModel(IUserConfirmation userConfirmation)
    {
        this.EngineAndDrivetrain = new SetupFieldListViewModel(userConfirmation);
        this.SteeringWheel = new SetupFieldListViewModel(userConfirmation);
        this.Suspension = new SetupFieldListViewModel(userConfirmation);
        this.TyresAndChassis = new SetupFieldListViewModel(userConfirmation);
    }

    public string CarId { get; private set; } = string.Empty;
    public SetupFieldListViewModel EngineAndDrivetrain { get; }
    public bool HasUnsavedChanges => this.CarId.Length > 0 && this.CurrentState() != this.savedState;
    public SetupFieldListViewModel SteeringWheel { get; }
    public SetupFieldListViewModel Suspension { get; }
    public SetupFieldListViewModel TyresAndChassis { get; }

    // Leaves the editor showing unsaved changes - e.g. a copy from another car.
    public void LoadFrom(string carId, PmrSetupMap? setupMap)
    {
        this.CarId = carId;
        this.EngineAndDrivetrain.LoadFrom(setupMap?.EngineAndDrivetrain ?? []);
        this.SteeringWheel.LoadFrom(setupMap?.SteeringWheel ?? []);
        this.Suspension.LoadFrom(setupMap?.Suspension ?? []);
        this.TyresAndChassis.LoadFrom(setupMap?.TyresAndChassis ?? []);
    }

    public void LoadSaved(string carId, PmrSetupMap? setupMap)
    {
        this.LoadFrom(carId, setupMap);
        this.MarkSaved();
    }

    public void MarkSaved()
    {
        this.savedState = this.CurrentState();
    }

    public PmrSetupMap ToSetupMap()
    {
        return new PmrSetupMap
        {
            CarId = this.CarId,
            EngineAndDrivetrain = this.EngineAndDrivetrain.ToSetupFieldInfos(),
            SteeringWheel = this.SteeringWheel.ToSetupFieldInfos(),
            Suspension = this.Suspension.ToSetupFieldInfos(),
            TyresAndChassis = this.TyresAndChassis.ToSetupFieldInfos()
        };
    }

    private string CurrentState()
    {
        return JsonSerializer.Serialize(this.ToSetupMap());
    }
}
