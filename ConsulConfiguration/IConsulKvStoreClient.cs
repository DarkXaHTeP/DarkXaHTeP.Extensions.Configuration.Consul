using System.Collections.Generic;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public interface IConsulKvStoreClient
    {
        Dictionary<string, string> ReadKeysRecursively();
    }
}