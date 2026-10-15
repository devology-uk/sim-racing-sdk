using SimRacingSdk.Lmu.Rest.Abstractions;

namespace SimRacingSdk.Lmu.Rest;

public class LmuRestClientFactory : ILmuRestClientFactory
{
    private static LmuRestClientFactory? singletonInstance;

    public static LmuRestClientFactory Instance => singletonInstance ??= new LmuRestClientFactory();

    public ILmuRestClient Create(string baseAddress = LmuRestClient.DefaultBaseAddress)
    {
        return new LmuRestClient(baseAddress);
    }
}
