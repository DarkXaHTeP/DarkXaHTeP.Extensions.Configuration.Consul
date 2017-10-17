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

        public ConsulKvStoreClient(HttpClient httpClient, string baseAddress, string prefix)
        {
            _httpClient = httpClient;
            _consulAddress = $"{baseAddress}/v1/kv/{prefix}?recurse=true";
        }
        
        public Dictionary<string, string> ReadKeysRecursively()
        {
            string response = _httpClient.GetStringAsync(_consulAddress).GetAwaiter().GetResult();
            var kvEntries = JsonConvert
                .DeserializeObject<ConsulKvStoreItem[]>(response)
                .Where(kv => !String.IsNullOrEmpty(kv.Value));

            var dictionary = kvEntries.ToDictionary(
                e => e.Key,
                e => DecodeValue(e.Value));

            return dictionary;
        }

        private string DecodeValue(string value)
        {
            byte[] data = Convert.FromBase64String(value);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
    }
}
