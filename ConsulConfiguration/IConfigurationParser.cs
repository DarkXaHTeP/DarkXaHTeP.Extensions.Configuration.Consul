using System.Collections.Generic;

namespace DarkXaHTeP.Extensions.Configuration.Consul
{
    public interface IConfigurationParser
    {
        Dictionary<string, string> ParseConfiguration(Dictionary<string, string> consulKvDictionary, string prefix);
    }
}