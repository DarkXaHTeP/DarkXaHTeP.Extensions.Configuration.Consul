using System.Net.Http;
using DarkXaHTeP.Extensions.Configuration.Consul.ConsulAddressProvider;
using DarkXaHTeP.Extensions.Configuration.Consul.ConsulClient;
using DarkXaHTeP.Extensions.Configuration.Consul.Parser;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationSource : IConfigurationSource
    {
        private readonly IConsulKvStoreClient _consulClient;
        private readonly IConfigurationParser _parser;

        internal ConsulConfigurationSource(string consulKey, string host, uint? port, IConsulAddressProvider consulAddressProvider, HttpClient httpClient)
        {
            var http = httpClient ?? new HttpClient();
            var addressProvider = consulAddressProvider ?? new DefaultConsulAddressProvider();

            var key = NormalizeConsulKey(consulKey);
            var consulBaseAddress = addressProvider.GetConsulBaseAddress(host, port);
            _consulClient = new ConsulKvStoreClient(http, consulBaseAddress, key);
            _parser = new ConfigurationParser(key);
        }
        
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(_consulClient, _parser);
        }
        
        private string NormalizeConsulKey(string consulKey)
        {
            string normalizedKey = consulKey;
            
            if (normalizedKey.StartsWith("/"))
            {
                normalizedKey = normalizedKey.Substring(1);
            }

            if (normalizedKey.EndsWith("/"))
            {
                normalizedKey = normalizedKey.Substring(0, normalizedKey.Length - 1);
            }

            return normalizedKey;
        }

    }
}