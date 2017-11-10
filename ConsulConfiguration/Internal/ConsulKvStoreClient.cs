using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace DarkXaHTeP.Extensions.Configuration.Consul.Internal
{
    internal class ConsulKvStoreClient: IConsulKvStoreClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _consulAddress;

        public ConsulKvStoreClient(HttpClient httpClient, string baseAddress, string prefix)
        {
            _httpClient = httpClient;
            _baseAddress = baseAddress;
            _consulAddress = $"{baseAddress}/v1/kv/{prefix}?recurse=true";
        }
        
        public Dictionary<string, string> ReadKeysRecursively()
        {
            HttpResponseMessage responseMessage;

            try
            {
                responseMessage = _httpClient.GetAsync(_consulAddress).GetAwaiter().GetResult();
            }
            catch (HttpRequestException e)
            {
                throw new ConsulConfigurationException($"Unable to connect to Consul using address: {_baseAddress}", e);
            }

            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                int statusCode = (int) responseMessage.StatusCode;
                string statusCodeDescription = responseMessage.StatusCode.ToString();
                
                throw new ConsulConfigurationException($"Consul response status code doesn't indicate success: {statusCode} {statusCodeDescription}");
            }

            string response = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            
            var kvEntries = JsonConvert
                .DeserializeObject<ConsulKvStoreItem[]>(response);

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
