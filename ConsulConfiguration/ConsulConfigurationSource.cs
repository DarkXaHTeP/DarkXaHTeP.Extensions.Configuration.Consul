using System.Net.Http;
using DarkXaHTeP.Extensions.Configuration.Consul.ConsulAddressProvider;
using DarkXaHTeP.Extensions.Configuration.Consul.ConsulClient;
using DarkXaHTeP.Extensions.Configuration.Consul.Parser;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationSource : IConfigurationSource
    {
        public string ConsulKey { get; set; }
        public string Host { get; set; }
        public uint? Port { get; set; }
        public IConsulAddressProvider ConsulAddressProvider { get; set; }
        public IConfigurationParser ConfigurationParser { get; set; }
        public HttpClient HttpClient { get; set; }

        internal ConsulConfigurationSource()
        {}
        
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults();
            return new ConsulConfigurationProvider(this);
        }

        private void EnsureDefaults()
        {
            ConsulAddressProvider = ConsulAddressProvider ?? new DefaultConsulAddressProvider();
            ConfigurationParser = ConfigurationParser ?? new ConfigurationParser();
            HttpClient = HttpClient ?? new HttpClient();
        }
        
        public void NormalizeConsulKey()
        {
            string normalizedKey = ConsulKey;
            
            if (normalizedKey.StartsWith("/"))
            {
                normalizedKey = normalizedKey.Substring(1);
            }

            if (normalizedKey.EndsWith("/"))
            {
                normalizedKey = normalizedKey.Substring(0, normalizedKey.Length - 1);
            }

            ConsulKey = normalizedKey;
        }

    }
}