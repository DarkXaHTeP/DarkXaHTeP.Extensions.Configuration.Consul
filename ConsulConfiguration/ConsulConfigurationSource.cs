using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationSource : IConfigurationSource
    {
        private readonly string _consulAddress;
        private readonly HttpClient _httpClient;

        internal ConsulConfigurationSource(string consulKey, string host, uint? port, HttpClient httpClient)
        {
            _httpClient = httpClient ?? new HttpClient();
            
            var key = NormalizeConsulKey(consulKey);
            var consulBaseAddress = ResolveBaseAddress(host, port);
            _consulAddress = $"http://{consulBaseAddress}/v1/kv/{key}";
        }
        
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(_consulAddress, _httpClient);
        }

        private string NormalizeConsulKey(string consulKey)
        {
            if (consulKey.StartsWith("/"))
            {
                return consulKey.Substring(1);
            }

            return consulKey;
        }
        
        private string ResolveBaseAddress(string host, uint? port)
        {
            var candidates = new[]
            {
                CreateAddress(host, port),
                GetEnvAddress(),
                "localhost:8500"
            };

            return candidates.First(addr => !String.IsNullOrEmpty(addr));
        }

        private string CreateAddress(string host, uint? port)
        {
            if (String.IsNullOrEmpty(host) || !port.HasValue)
            {
                return null;
            }

            return $"{host}:{port}";
        }

        private string GetEnvAddress()
        {
            string host = Environment.GetEnvironmentVariable("CONSUL_HOST");
            string portString = Environment.GetEnvironmentVariable("CONSUL_PORT");
            
            if (String.IsNullOrEmpty(host) || String.IsNullOrEmpty(portString))
            {
                return null;
            }

            if (!UInt32.TryParse(portString, out var port))
            {
                throw new ArgumentException("Cannot parse ENV variable \"CONSUL_PORT\". It's value is not a valid port");
            }

            return CreateAddress(host, port);
        }
    }
}