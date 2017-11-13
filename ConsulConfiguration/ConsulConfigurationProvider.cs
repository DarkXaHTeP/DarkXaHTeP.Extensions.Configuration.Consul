using System.Collections.Generic;
using DarkXaHTeP.Extensions.Configuration.Consul.Internal;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationProvider: ConfigurationProvider
    {
        private readonly IConsulKvStoreClient _consulClient;
        private readonly IConfigurationParser _parser;
        private readonly string _consulKey;

        internal ConsulConfigurationProvider(ConsulConfigurationSource source)
        {
            string consulAddress = source.ConsulAddressProvider.GetBaseAddress(source.Host, source.Port);

            _consulKey = source.ConsulKey;
            
            _consulClient = new ConsulKvStoreClient(source.HttpClient, consulAddress, _consulKey);
            _parser = source.ConfigurationParser;
        }

        public override void Load()
        {
            Dictionary<string, string> consulKeys = _consulClient.ReadKeysRecursively();
            Dictionary<string, string> config = _parser.ParseConfiguration(consulKeys, _consulKey);
            foreach (var kvPair in config)
            {
                Set(kvPair.Key, kvPair.Value);
            }
        }
    }
}