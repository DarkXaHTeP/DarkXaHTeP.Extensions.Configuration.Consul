using System;
using System.Linq;

namespace DarkXaHTeP.Extensions.Configuration.Consul.Internal
{
    internal class DefaultConsulAddressProvider: IConsulAddressProvider
    {
        public string GetBaseAddress(string host, uint? port)
        {
            var consulBaseAddress = ResolveBaseAddress(host, port);
            return $"http://{consulBaseAddress}";
        }
        
        private string ResolveBaseAddress(string host, uint? port)
        {
            var hostCandidates = new[]
            {
                host,
                GetEnvVar("CONSUL_HOST"),
                "localhost"
            };

            var portCandidates = new uint?[]
            {
                port,
                GetEnvPort(),
                8500
            };

            return CreateAddress(First(hostCandidates), First(portCandidates));
        }

        private string CreateAddress(string host, uint port)
        {
            return $"{host}:{port}";
        }

        private uint? GetEnvPort()
        {
            string portString = GetEnvVar("CONSUL_PORT");
            if (String.IsNullOrEmpty(portString))
            {
                return null;
            }
            
            if (!UInt32.TryParse(portString, out var port))
            {
                throw new ArgumentException("Cannot parse ENV variable \"CONSUL_PORT\". It's value is not a valid port");
            }

            return port;
        }
        
        private string GetEnvVar(string envVar)
        {
            return Environment.GetEnvironmentVariable(envVar);
        }

        private string First(string[] candidates)
        {
            return candidates.First(val => !String.IsNullOrEmpty(val));
        }

        private uint First(uint?[] candidates)
        {
            var candidate = candidates.First(val => val.HasValue);

            return candidate.Value;
        }
    }
}
