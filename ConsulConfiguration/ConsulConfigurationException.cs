using System;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public class ConsulConfigurationException: Exception
    {
        public ConsulConfigurationException(string message, Exception innerException): base(message, innerException)
        {
        }

        public ConsulConfigurationException(string message): base(message)
        {
        }
    }
}