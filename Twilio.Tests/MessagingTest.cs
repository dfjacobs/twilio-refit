using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using Xunit;
using Xunit.Abstractions;
// ReSharper disable PossibleNullReferenceException

namespace Twilio.Tests
{
    public class MessagingTest
    {
        private const string ACCOUNTSID = "TWILIO_ACCOUNTSID";
        private const string AUTHTOKEN = "TWILIO_AUTHTOKEN";
        private const string PHONENUMBER = "TWILIO_PHONENUMBER";

        private readonly ITwilioMessagingApi api;
        private readonly string accountSid;
        private readonly string fromNumber;

        private readonly ITestOutputHelper testOutput;

        public MessagingTest(ITestOutputHelper output)
        {
            testOutput = output;
            DotNetEnv.Env.Load("../../../.env");

            accountSid = Environment.GetEnvironmentVariable(ACCOUNTSID);

            var twilioAuthentication = new TwilioAuthorization(
                accountSid,
                Environment.GetEnvironmentVariable(AUTHTOKEN)
            );

            fromNumber = Environment.GetEnvironmentVariable(PHONENUMBER);

            var messagingClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.twilio.com"),
                DefaultRequestHeaders = { Authorization = twilioAuthentication.AuthorizationHeader }
            };
            api = RestService.For<ITwilioMessagingApi>(messagingClient);
        }

        [Fact]
        public async Task SendSmsToValidToAddress()
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    {"To", "+17202950840"},
                    {"From", fromNumber},
                    {"Body", "Test message"}
                };
                dynamic response = await api.SendSMS(accountSid, parameters);

                Assert.Equal(accountSid, (string)response.account_sid);
                Assert.Equal("queued", (string)response.status);
            }
            catch (ApiException ex)
            {
                testOutput.WriteLine("SendSMS request failed:");
                if (ex.HasContent)
                {
                    testOutput.WriteLine(ex.Content);
                }
                throw;
            }
        }

        [Theory]
        [InlineData("+15005550001")]
        [InlineData("+15005550002")]
        [InlineData("+15005550003")]
        [InlineData("+15005550004")]
        [InlineData("+15005550009")]
        public async Task SendSmsToInvalidToAddress(string value)
        {
            var parameters = new Dictionary<string, string>
            {
                {"To", value},
                {"From", fromNumber},
                {"Body", "Test message"}
            };
            var exception = await Record.ExceptionAsync(() => api.SendSMS(accountSid, parameters));
            Assert.NotNull(exception);
            Assert.IsType<ApiException>(exception);
            testOutput.WriteLine((exception as ApiException).Content);
        }

        [Theory]
        [InlineData("+15005550001")]
        [InlineData("+15005550007")]
        [InlineData("+15005550008")]
        public async Task SendSmsFromInvalidToAddress(string value)
        {
            var parameters = new Dictionary<string, string>
            {
                {"To", "+17202950840"},
                {"From", value},
                {"Body", "Test message"}
            };
            var exception = await Record.ExceptionAsync(() => api.SendSMS(accountSid, parameters));
            Assert.NotNull(exception);
            Assert.IsType<ApiException>(exception);
            testOutput.WriteLine((exception as ApiException).Content);
        }
    }
}