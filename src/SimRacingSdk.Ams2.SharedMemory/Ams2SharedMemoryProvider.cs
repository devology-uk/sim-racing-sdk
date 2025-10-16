using System.Diagnostics;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;
using SimRacingSdk.Ams2.SharedMemory.Messages;
using SimRacingSdk.Ams2.SharedMemory.Models;

namespace SimRacingSdk.Ams2.SharedMemory;

public class Ams2SharedMemoryProvider : IAms2SharedMemoryProvider
{
    private static Ams2SharedMemoryProvider? singletonInstance;

    public static Ams2SharedMemoryProvider Instance => singletonInstance ??= new Ams2SharedMemoryProvider();

    public SharedMemoryData ReadSharedMemoryData()
    {
        try
        {
            var page = SharedMemoryPage.Read();
            return page == null? new SharedMemoryData(): new SharedMemoryData(page);
        }
        catch(Exception exception)
        {
            Debug.WriteLine(exception);
            return new SharedMemoryData();
        }
    }
}