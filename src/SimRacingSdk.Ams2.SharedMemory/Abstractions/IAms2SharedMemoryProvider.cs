using SimRacingSdk.Ams2.SharedMemory.Messages;
using SimRacingSdk.Ams2.SharedMemory.Models;

namespace SimRacingSdk.Ams2.SharedMemory.Abstractions;

public interface IAms2SharedMemoryProvider
{
    SharedMemoryPage ReadSharedMemoryPage();
    SharedMemoryData ReadSharedMemoryData();
}