using System;

namespace DarkXaHTeP.Extensions.Configuration.Consul.ConsulAddressProvider
{
    public interface IConsulAddressProvider
    {
        string GetConsulBaseAddress(string host, uint? port);
    }
}
