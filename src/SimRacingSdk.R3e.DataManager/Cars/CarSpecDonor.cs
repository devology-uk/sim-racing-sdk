namespace SimRacingSdk.R3e.DataManager.Cars;

// A car whose captured spec can be copied onto the same car in another class.
public record CarSpecDonor(CarInfo Car)
{
    public string Label => $"{this.Car.Name} ({this.Car.Class}) - {this.Car.PowerBhp} bhp, {this.Car.WeightWithDriverKg} kg";

    public static IReadOnlyList<CarSpecDonor> From(IEnumerable<CarInfo> cars)
    {
        return cars.Where(HasSpec)
            .Select(car => new CarSpecDonor(car))
            .OrderBy(donor => donor.Label)
            .ToList();
    }

    private static bool HasSpec(CarInfo car)
    {
        return car.PowerBhp is not null || car.WeightWithDriverKg is not null || car.EngineCapacityLitres is not null;
    }
}
