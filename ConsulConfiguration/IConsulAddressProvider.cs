namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public interface IConsulAddressProvider
    {
        string GetBaseAddress(string host, uint? port);
    }
}
