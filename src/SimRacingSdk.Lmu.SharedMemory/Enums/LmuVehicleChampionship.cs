namespace SimRacingSdk.Lmu.SharedMemory.Enums;

// Matches IP_VehicleChampionship (InternalsPlugin.hpp).
public enum LmuVehicleChampionship : byte
{
    Wec2023 = 0x00,
    Wec2024 = 0x01,
    Wec2025 = 0x02,
    Wec2026 = 0x03,
    Elms2025 = 0x10,
    Elms2026 = 0x11,
    Unknown = 0xFF
}
