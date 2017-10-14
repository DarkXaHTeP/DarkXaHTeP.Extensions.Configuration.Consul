using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DarkXaHTeP.Extensions.Configuration.Consul.Parser
{
    public class ConfigurationParser: IConfigurationParser
    {
        private readonly Regex _arrayRegex = new Regex(@"\[\d+\]$");
        private readonly string _keyDelimiter = ":";
        private readonly string _prefix;

        public ConfigurationParser(string prefix)
        {
            _prefix = prefix;
        }

        public Dictionary<string, string> ParseConfiguration(Dictionary<string, string> consulKvDictionary)
        {
            Dictionary<string, string> results = consulKvDictionary
                .Select(kv => ParseKey(kv.Key, kv.Value))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            return results;
        }

        private KeyValuePair<string, string> ParseKey(string key, string value)
        {
            var keyParts = RemovePrefix(key).Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries);

            if (keyParts.Length == 0)
            {
                throw new ArgumentException($"Consul key {key} is incorrect", nameof(key));
            }

            var configurationKeyParts = keyParts.Select(ConvertKeyPart);
            
            var configurationKey = String.Join(_keyDelimiter, configurationKeyParts);
            
            return new KeyValuePair<string, string>(configurationKey, value);
        }

        private string RemovePrefix(string key)
        {
            return key.Substring(_prefix.Length);
        }

        private string ConvertKeyPart(string keyPrefix)
        {
            if (_arrayRegex.IsMatch(keyPrefix))
            {
                var openBracketIndex = keyPrefix.LastIndexOf('[');
                var replaced = keyPrefix
                                   .Remove(openBracketIndex, 1)
                                   .Insert(openBracketIndex, _keyDelimiter)
                                   .Substring(0, keyPrefix.Length - 1);

                return replaced;

            }

            return keyPrefix;
        }
    }
}
