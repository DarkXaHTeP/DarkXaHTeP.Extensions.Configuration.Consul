using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string consulKey)
        {
            return AddConsul(builder, consulKey, null, null);
        }
        
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string consulKey, string host, uint? port)
        {
            return AddConsul(builder, consulKey, host, port, null);
        }
        
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string consulKey, string host, uint? port, HttpClient httpClient)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(consulKey))
            {
                throw new ArgumentException("Consul Key cannot be null", nameof(consulKey));
            }
            
            return builder.Add(new ConsulConfigurationSource(consulKey, host, port, httpClient));
        }
    }
}
