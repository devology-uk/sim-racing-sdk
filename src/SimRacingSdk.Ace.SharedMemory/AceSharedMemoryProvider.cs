using System.Diagnostics;
using SimRacingSdk.Ace.SharedMemory.Abstractions;
using SimRacingSdk.Ace.SharedMemory.Messages;
using SimRacingSdk.Ace.SharedMemory.Models;

namespace SimRacingSdk.Ace.SharedMemory;

public class AceSharedMemoryProvider : IAceSharedMemoryProvider
{
    private static AceSharedMemoryProvider? singletonInstance;

    public static AceSharedMemoryProvider Instance => singletonInstance ??= new AceSharedMemoryProvider();

    public AceGraphicsData ReadGraphicsData()
    {
        try
        {
            var page = AceGraphicsPage.Read();
            return page == null ? new AceGraphicsData() : new AceGraphicsData(page);
        }
        catch(Exception exception)
        {
            Debug.WriteLine(exception);
            return new AceGraphicsData();
        }
    }

    public AcePhysicsData ReadPhysicsData()
    {
        try
        {
            var page = AcePhysicsPage.Read();
            return page == null ? new AcePhysicsData() : new AcePhysicsData(page);
        }
        catch(Exception exception)
        {
            Debug.WriteLine(exception);
            return new AcePhysicsData();
        }
    }

    public AceStaticData ReadStaticData()
    {
        try
        {
            var page = AceStaticDataPage.Read();
            return page == null ? new AceStaticData() : new AceStaticData(page);
        }
        catch(Exception exception)
        {
            Debug.WriteLine(exception);
            return new AceStaticData();
        }
    }
}
