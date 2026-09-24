namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public class SetupMapEditorViewModel
{
    private string carId = string.Empty;

    public SetupFieldListViewModel EngineAndDrivetrain { get; } = new();
    public SetupFieldListViewModel SteeringWheel { get; } = new();
    public SetupFieldListViewModel Suspension { get; } = new();
    public SetupFieldListViewModel TyresAndChassis { get; } = new();

    public void LoadFrom(string carId, PmrSetupMap? setupMap)
    {
        this.carId = carId;
        this.EngineAndDrivetrain.LoadFrom(setupMap?.EngineAndDrivetrain ?? []);
        this.SteeringWheel.LoadFrom(setupMap?.SteeringWheel ?? []);
        this.Suspension.LoadFrom(setupMap?.Suspension ?? []);
        this.TyresAndChassis.LoadFrom(setupMap?.TyresAndChassis ?? []);
    }

    public PmrSetupMap ToSetupMap()
    {
        return new PmrSetupMap
        {
            CarId = this.carId,
            EngineAndDrivetrain = this.EngineAndDrivetrain.ToSetupFieldInfos(),
            SteeringWheel = this.SteeringWheel.ToSetupFieldInfos(),
            Suspension = this.Suspension.ToSetupFieldInfos(),
            TyresAndChassis = this.TyresAndChassis.ToSetupFieldInfos()
        };
    }
}
