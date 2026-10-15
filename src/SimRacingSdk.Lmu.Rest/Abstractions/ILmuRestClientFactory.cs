namespace SimRacingSdk.Lmu.Rest.Abstractions;

public interface ILmuRestClientFactory
{
    ILmuRestClient Create(string baseAddress = LmuRestClient.DefaultBaseAddress);
}
