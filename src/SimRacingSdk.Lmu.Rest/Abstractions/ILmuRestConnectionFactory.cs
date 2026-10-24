namespace SimRacingSdk.Lmu.Rest.Abstractions;

public interface ILmuRestConnectionFactory
{
    ILmuRestConnection Create(string baseAddress = LmuRestClient.DefaultBaseAddress);
}
