using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using RichardSzalay.MockHttp;
using Xunit;

namespace DarkXaHTeP.Extensions.Configuration.Consul.Test
{
    public class ConsulConfigurationErrorTest
    {
        private static readonly string ConsulTestKey = "service";
        private static readonly string ConsulAddress = $"http://localhost:8500/v1/kv/{ConsulTestKey}?recurse=true";
        
            
        [Fact]
        public void ShouldThrowIfCannotConnect()
        {
            var exception = new HttpRequestException("Unable to connect to server");
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When(ConsulAddress)
                .Throw(exception);

            var builder = new ConfigurationBuilder()
                .AddConsul(ConsulTestKey, null, null, mockHttp.ToHttpClient());

            try
            {
                builder.Build();
                
                throw new Exception("Should not reach this code");
            }
            catch (ConsulConfigurationException ex)
            {
                Assert.Equal(exception, ex.InnerException);
            }
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound, "404 NotFound")]
        [InlineData(HttpStatusCode.InternalServerError, "500 InternalServerError")]
        public void ShouldThrowOnNonSuccessStatusCodes(HttpStatusCode code, string messageError)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ConsulAddress)
                .Respond(code);
            
            var builder = new ConfigurationBuilder()
                .AddConsul(ConsulTestKey, null, null, mockHttp.ToHttpClient());

            try
            {
                builder.Build();
                
                throw new Exception("Should not reach this code");
            }
            catch (ConsulConfigurationException ex)
            {
                Assert.Null(ex.InnerException);
                Assert.Equal($"Consul response status code doesn't indicate success: {messageError}", ex.Message);
            }
        }
    }
}