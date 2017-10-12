using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace DarkXaHTeP.Extensions.Configuration.Consul.ConsulClient
{
    public class ConsulKvStoreClient: IConsulKvStoreClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _consulAddress;
        private readonly string _prefix;

        public ConsulKvStoreClient(HttpClient httpClient, string baseAddress, string prefix)
        {
            _prefix = prefix;
            _httpClient = httpClient;
            _consulAddress = $"{baseAddress}/v1/kv/{prefix}?recurse=true";
        }
        
        public Dictionary<string, string> ReadKeysRecursively()
        {
            string response = _httpClient.GetStringAsync(_consulAddress).GetAwaiter().GetResult();
            var kvEntries = JsonConvert.DeserializeObject<ConsulKvStoreItem[]>(response);
            var dictionary = kvEntries.ToDictionary(
                e => NormalizeKey(e.Key),
                e => DecodeValue(e.Value));

            return dictionary;
        }

        private string DecodeValue(string value)
        {
            byte[] data = Convert.FromBase64String(value);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        private string NormalizeKey(string key)
        {
            var lettersToDrop = $"{_prefix}/".Length;
            return key.Substring(lettersToDrop);
        }
    }
}
