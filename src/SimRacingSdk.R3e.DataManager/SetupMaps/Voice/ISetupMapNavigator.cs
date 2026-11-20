namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// What voice entry can do on the Setup Maps page beyond editing the rows.
public interface ISetupMapNavigator
{
    SetupMapCarRow? CurrentCar { get; }
    SetupMapEditorViewModel Editor { get; }
    SetupMapCarRow? PreviousCar { get; }

    void CopyFrom(SetupMapCarRow sourceCar);

    // Returns null once saved, otherwise why it couldn't be.
    string? SaveMap();

    bool SelectCarNamed(string carName);
    bool SelectNextCar();
    bool SelectPreviousCar();
}
