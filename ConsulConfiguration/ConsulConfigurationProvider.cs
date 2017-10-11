using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationProvider: ConfigurationProvider
    {
        private readonly string _consulAddress;
        private readonly HttpClient _httpClient;

        internal ConsulConfigurationProvider(string consulAddress, HttpClient httpClient)
        {
            _consulAddress = consulAddress;
            _httpClient = httpClient;
        }

        public override void Load()
        {
            
        }
    }
}