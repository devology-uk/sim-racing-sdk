using System.Diagnostics;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Messages;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccSharedMemoryProvider : IAccSharedMemoryProvider
{
    private static AccSharedMemoryProvider? singletonInstance;

    public static AccSharedMemoryProvider Instance => singletonInstance ??= new AccSharedMemoryProvider();

    public GraphicsData ReadGraphicsData()
    {
        try
        {
            var page = GraphicsPage.Read();
            return page == null? new GraphicsData(): new GraphicsData(page);
        }
        catch(Exception exception)
        {
            Debug.WriteLine(exception);
            return new GraphicsData();
        }
    }

    public PhysicsData ReadPhysicsData()
    {
        try
        {
            var page = PhysicsPage.Read();
            return page == null? new PhysicsData(): new PhysicsData(page);
        }
        catch(Exception exception)
        {
            Debug.WriteLine(exception);
            return new PhysicsData();
        }
    }

    public StaticData ReadStaticData()
    {
        try
        {
            var page = StaticDataPage.Read();
            return page == null? new StaticData(): new StaticData(page);
        }
        catch(Exception exception)
        {
            Debug.WriteLine(exception);
            return new StaticData();
        }
    }
}