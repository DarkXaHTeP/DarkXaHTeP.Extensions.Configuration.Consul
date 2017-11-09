using System.Collections.Generic;
using System.Linq;
using DarkXaHTeP.Extensions.Configuration.Consul.Internal;
using RichardSzalay.MockHttp;
using Xunit;

namespace DarkXaHTeP.Extensions.Configuration.Consul.Test
{
    public class ConsulClientTest
    {
        [Fact]
        public void ShouldReturnKvStoreValues()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://localhost/v1/kv/service?recurse=true")
                .Respond(
                    "application/json",
                    "[{\"LockIndex\": 0,\"Key\": \"service/testkey\",\"Flags\": 0,\"Value\": \"dGVzdHZhbHVl\",\"CreateIndex\": 2196,\"ModifyIndex\": 2196}]"
                );

            var client = new ConsulKvStoreClient(
                mockHttp.ToHttpClient(),
                "http://localhost",
                "service"
            );

            Dictionary<string, string> result = client.ReadKeysRecursively();
            Assert.Single(result);

            var kvPair = result.Single();
            
            Assert.Equal("service/testkey", kvPair.Key);
            Assert.Equal("testvalue", kvPair.Value);
        }
    }
}