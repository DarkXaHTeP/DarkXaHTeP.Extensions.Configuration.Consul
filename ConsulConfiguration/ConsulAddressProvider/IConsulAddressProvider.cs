using System;

namespace DarkXaHTeP.Extensions.Configuration.Consul.ConsulAddressProvider
{
    public interface IConsulAddressProvider
    {
        string GetBaseAddress(string host, uint? port);
    }
}
