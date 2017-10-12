using DarkXaHTeP.Extensions.Configuration.Consul.ConsulClient;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationProvider: ConfigurationProvider
    {
        private readonly IConsulKvStoreClient _consulClient;

        internal ConsulConfigurationProvider(IConsulKvStoreClient consulClient)
        {
            _consulClient = consulClient;
        }

        public override void Load()
        {
            
        }
    }
}