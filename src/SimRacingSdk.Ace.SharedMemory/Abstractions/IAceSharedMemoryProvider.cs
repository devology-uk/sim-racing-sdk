using SimRacingSdk.Ace.SharedMemory.Models;

namespace SimRacingSdk.Ace.SharedMemory.Abstractions;

public interface IAceSharedMemoryProvider
{
    AceGraphicsData ReadGraphicsData();
    AcePhysicsData ReadPhysicsData();
    AceStaticData ReadStaticData();
}
