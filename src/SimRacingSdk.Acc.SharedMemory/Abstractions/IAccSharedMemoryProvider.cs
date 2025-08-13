using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccSharedMemoryProvider
{
    GraphicsData? ReadGraphicsData();
    PhysicsData? ReadPhysicsData();
    StaticData? ReadStaticData();
}