namespace SimRacingSdk.Lmu.SharedMemory.Enums;

// Matches IP_VehicleClass (InternalsPlugin.hpp).
public enum LmuVehicleClass : byte
{
    Hypercar = 0x00,
    Lmp2Elms = 0x02,
    Lmp2 = 0x03,
    Lmp3 = 0x04,
    Gte = 0x05,
    Gt3 = 0x06,
    PaceCar = 0x08,
    Unknown = 0xFF
}
