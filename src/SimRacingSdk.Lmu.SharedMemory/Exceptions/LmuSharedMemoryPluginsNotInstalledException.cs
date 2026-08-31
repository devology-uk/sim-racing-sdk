namespace SimRacingSdk.Lmu.SharedMemory.Exceptions;

public class LmuSharedMemoryPluginsNotInstalledException : Exception
{
    public LmuSharedMemoryPluginsNotInstalledException()
        : base(
            "Both LMU_SharedMemoryMapPlugin64.dll and rFactor2SharedMemoryMapPlugin64.dll must be installed and configured before starting an LMU shared memory connection.")
    {
    }
}
