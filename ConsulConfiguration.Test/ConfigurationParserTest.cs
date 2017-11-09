using System.Collections.Generic;
using DarkXaHTeP.Extensions.Configuration.Consul.Internal;
using Xunit;

namespace DarkXaHTeP.Extensions.Configuration.Consul.Test
{
    public class ConfigurationParserTest
    {
        [Theory]
        [InlineData("testkey", "testkey")]
        [InlineData("subkey/testkey", "subkey:testkey")]
        [InlineData("testkey[0]", "testkey:0")]
        [InlineData("testkey[111]", "testkey:111")]
        [InlineData("subkey[0]/testkey", "subkey:0:testkey")]
        public void ShouldParseKeys(string consulKey, string configurationKey)
        {
            const string consulPrefix = "app";
            const string testValue = "testvalue";

            var consulData = new Dictionary<string, string>
            {
                {$"{consulPrefix}/{consulKey}", testValue}
            };

            var parser = new ConfigurationParser();

            var result = parser.ParseConfiguration(consulData, consulPrefix);

            Assert.Single(result);
            Assert.True(result.ContainsKey(configurationKey), "Parsed configuration doesn't contain key");
            Assert.Equal(testValue, result[configurationKey]);
        }
    }
}
