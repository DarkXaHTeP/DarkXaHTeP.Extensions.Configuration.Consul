using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using RichardSzalay.MockHttp;
using Xunit;

namespace DarkXaHTeP.Extensions.Configuration.Consul.Test
{
    public class StringSettings
    {
        public string StringProp { get; set; }
    }

    public class BoolSettings
    {
        public bool TrueBoolProp { get; set; }
        public bool FalseBoolProp { get; set; }
    }
    
    public class ObjectSettings
    {
        public StringSettings SubKey { get; set; }
    }

    public class StringArraySettings
    {
        public string[] ArrayKeys { get; set; }
    }

    public class ObjectArraySettings
    {
        public StringSettings[] ArrayObjects { get; set; }
    }

    public class NumericSettings
    {
        public int IntProp { get; set; }
        public float FloatProp { get; set; }
    }
    
    public class ConsulConfigurationExtensionTest
    {
        private readonly IConfigurationBuilder _builder;

        public ConsulConfigurationExtensionTest()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var binFolder = Path.GetDirectoryName(assemblyPath);
            var testDataPath = Path.Combine(binFolder, "TestData", "ConsulJson.json");

            var jsonResponse = File.ReadAllText(testDataPath);
            
            var mockHttp = new MockHttpMessageHandler();
            
            mockHttp
                .When("http://localhost:8500/v1/kv/TestApp?recurse=true")
                .Respond("application/json", jsonResponse);

            _builder = new ConfigurationBuilder()
                .AddConsul("TestApp", null, null, mockHttp.ToHttpClient());
        }

        [Fact]
        public void ShouldMapString()
        {
            var config = GetConfig<StringSettings>();
            
            Assert.Equal("StringValue", config.StringProp);
        }

        [Fact]
        public void ShouldMapBool()
        {
            var config = GetConfig<BoolSettings>();
            
            Assert.True(config.TrueBoolProp);
            Assert.False(config.FalseBoolProp);
        }

        [Fact]
        public void ShouldMapObject()
        {
            var config = GetConfig<ObjectSettings>();
            
            Assert.Equal("StringValue2", config.SubKey.StringProp);
        }

        [Fact]
        public void ShouldMapStringArray()
        {
            var config = GetConfig<StringArraySettings>();
            
            Assert.Equal(3, config.ArrayKeys.Length);
            Assert.Equal("array0", config.ArrayKeys[0]);
            Assert.Equal("array1", config.ArrayKeys[1]);
            Assert.Equal("array2", config.ArrayKeys[2]);
        }

        [Fact]
        public void ShouldMapObjectArray()
        {
            var config = GetConfig<ObjectArraySettings>();
            
            Assert.Equal(2, config.ArrayObjects.Length);
            Assert.Equal("array string 0", config.ArrayObjects[0].StringProp);
            Assert.Equal("array string 1", config.ArrayObjects[1].StringProp);
        }

        [Fact]
        public void ShouldMapNumbers()
        {
            var config = GetConfig<NumericSettings>();
            
            Assert.Equal(1032, config.IntProp);
            Assert.Equal(1.001, config.FloatProp, 6);
        }

        private T GetConfig<T>()
        {
            var config = _builder.Build();
            return config.Get<T>();
        }
    }
}
