using SimRacingSdk.Lmu.Rest.Abstractions;

namespace SimRacingSdk.Lmu.Rest;

public class LmuRestConnectionFactory : ILmuRestConnectionFactory
{
    private static LmuRestConnectionFactory? singletonInstance;

    private readonly ILmuRestClientFactory restClientFactory;

    public LmuRestConnectionFactory(ILmuRestClientFactory restClientFactory)
    {
        this.restClientFactory = restClientFactory;
    }

    public static LmuRestConnectionFactory Instance =>
        singletonInstance ??= new LmuRestConnectionFactory(LmuRestClientFactory.Instance);

    public ILmuRestConnection Create(string baseAddress = LmuRestClient.DefaultBaseAddress)
    {
        return new LmuRestConnection(this.restClientFactory.Create(baseAddress));
    }
}
