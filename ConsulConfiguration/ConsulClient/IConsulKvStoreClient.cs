using System.Collections.Generic;

namespace DarkXaHTeP.Extensions.Configuration.Consul.ConsulClient
{
    public interface IConsulKvStoreClient
    {
        Dictionary<string, string> ReadKeysRecursively();
    }
}