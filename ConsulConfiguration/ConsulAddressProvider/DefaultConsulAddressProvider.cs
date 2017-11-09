using System;
using System.Linq;

namespace DarkXaHTeP.Extensions.Configuration.Consul.ConsulAddressProvider
{
    public class DefaultConsulAddressProvider: IConsulAddressProvider
    {
        public string GetBaseAddress(string host, uint? port)
        {
            var consulBaseAddress = ResolveBaseAddress(host, port);
            return $"http://{consulBaseAddress}";
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