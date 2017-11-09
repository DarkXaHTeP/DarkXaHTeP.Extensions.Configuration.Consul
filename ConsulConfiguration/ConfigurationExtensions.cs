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
            return AddConsul(
                builder,
                source =>
                {
                    source.ConsulKey = consulKey;
                    source.Host = host;
                    source.Port = port;
                    source.HttpClient = httpClient;
                });
        }

        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, Action<ConsulConfigurationSource> configureSource)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var source = new ConsulConfigurationSource();
            configureSource(source);

            if (String.IsNullOrEmpty(source.ConsulKey))
            {
                throw new ArgumentNullException("Consul Key");
            }
            
            source.NormalizeConsulKey();
            
            return builder.Add(source);
        }
    }
}
