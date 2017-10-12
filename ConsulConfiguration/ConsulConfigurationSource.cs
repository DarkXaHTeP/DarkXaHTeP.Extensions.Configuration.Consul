using System.Net.Http;
using DarkXaHTeP.Extensions.Configuration.Consul.ConsulAddressProvider;
using DarkXaHTeP.Extensions.Configuration.Consul.ConsulClient;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationSource : IConfigurationSource
    {
        private readonly IConsulKvStoreClient _consulClient;

        internal ConsulConfigurationSource(string consulKey, string host, uint? port, IConsulAddressProvider consulAddressProvider, HttpClient httpClient)
        {
            var http = httpClient ?? new HttpClient();
            var addressProvider = consulAddressProvider ?? new DefaultConsulAddressProvider();

            var key = NormalizeConsulKey(consulKey);
            var consulBaseAddress = addressProvider.GetConsulBaseAddress(host, port);
            _consulClient = new ConsulKvStoreClient(http, consulBaseAddress, key);
        }
        
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(_consulClient);
        }
        
        private string NormalizeConsulKey(string consulKey)
        {
            if (consulKey.StartsWith("/"))
            {
                return consulKey.Substring(1);
            }

            return consulKey;
        }

    }
}