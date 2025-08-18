using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Messages;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccSharedMemoryProvider : IAccSharedMemoryProvider
{
    private static AccSharedMemoryProvider? singletonInstance;

    public static AccSharedMemoryProvider Instance => singletonInstance ??= new AccSharedMemoryProvider();

    public GraphicsData? ReadGraphicsData()
    {
        return GraphicsPage.TryRead(out var graphicsPage)? new GraphicsData(graphicsPage): null;
    }

    public PhysicsData? ReadPhysicsData()
    {
        return PhysicsPage.TryRead(out var physicsPage)? new PhysicsData(physicsPage): null;
    }

    public StaticData? ReadStaticData()
    {
        return StaticDataPage.TryRead(out var staticDataPage)? new StaticData(staticDataPage): null;
    }
}