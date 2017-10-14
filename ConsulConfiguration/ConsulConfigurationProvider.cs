using System.Collections.Generic;
using DarkXaHTeP.Extensions.Configuration.Consul.ConsulClient;
using DarkXaHTeP.Extensions.Configuration.Consul.Parser;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationProvider: ConfigurationProvider
    {
        private readonly IConsulKvStoreClient _consulClient;
        private readonly IConfigurationParser _parser;

        internal ConsulConfigurationProvider(IConsulKvStoreClient consulClient, IConfigurationParser parser)
        {
            _consulClient = consulClient;
            _parser = parser;
        }

        public override void Load()
        {
            Dictionary<string, string> consulData = _consulClient.ReadKeysRecursively();
            Dictionary<string, string> config = _parser.ParseConfiguration(consulData);
            Data = config;
        }
    }
}