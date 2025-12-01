using SimRacingSdk.Acc.SharedMemory.Enums;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public record AccAppStatusChange(AccAppStatus From, AccAppStatus To)
{ }