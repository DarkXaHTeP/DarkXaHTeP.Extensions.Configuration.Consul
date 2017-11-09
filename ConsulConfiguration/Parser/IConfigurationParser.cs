using System.Collections.Generic;

namespace DarkXaHTeP.Extensions.Configuration.Consul.Parser
{
    public interface IConfigurationParser
    {
        Dictionary<string, string> ParseConfiguration(Dictionary<string, string> consulKvDictionary, string prefix);
    }
}