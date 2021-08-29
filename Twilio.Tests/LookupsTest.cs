using System;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using Xunit;
using Xunit.Abstractions;

namespace Twilio.Tests
{
    public class LookupsTest
    {
        private const string ACCOUNTSID = "TWILIO_ACCOUNTSID";
        private const string AUTHTOKEN = "TWILIO_AUTHTOKEN";

        private readonly ITwilioLookupsApi api;
        private readonly ITestOutputHelper testOutput;

        public LookupsTest(ITestOutputHelper output)
        {
            testOutput = output;
            DotNetEnv.Env.Load("../../../.env");

            var twilioAuthentication = new TwilioAuthorization(
                Environment.GetEnvironmentVariable(ACCOUNTSID),
                Environment.GetEnvironmentVariable(AUTHTOKEN)
            );

            var lookupsClient = new HttpClient
            {
                BaseAddress = new Uri("https://lookups.twilio.com/v1"),
                DefaultRequestHeaders = { Authorization = twilioAuthentication.AuthorizationHeader }
            };
            api = RestService.For<ITwilioLookupsApi>(lookupsClient);
        }

        [Fact]
        public async Task LookupCarrierInformation()
        {
            try
            {
                var response = await api.NumberInfo("+17202950840", "US", "carrier");
                Assert.Equal("mobile", response.GetProperty("carrier").GetProperty("type").GetString());
            }
            catch (ApiException ex)
            {
                testOutput.WriteLine("NumberInfo request failed:");
                if (ex.HasContent)
                {
                    testOutput.WriteLine(ex.Content);
                }
                throw;
            }
        }
    }
}