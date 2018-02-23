using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using Xunit;
using Xunit.Abstractions;

namespace Twilio.Tests
{
    public class RestApiTest
    {
        private const string ACCOUNTSID = "TWILIO_ACCOUNTSID_TEST";
        private const string AUTHTOKEN = "TWILIO_AUTHTOKEN_TEST";
        private const string PHONENUMBER = "TWILIO_PHONENUMBER";

        private readonly ITwilioRestApi api;
        private readonly ITestOutputHelper testOutput;
        private readonly string accountSid;
        private readonly string fromNumber;

        public RestApiTest(ITestOutputHelper output)
        {
            testOutput = output;
            DotNetEnv.Env.Load("../../../.env");

            accountSid = Environment.GetEnvironmentVariable(ACCOUNTSID);

            var twilioAuthentication = new TwilioAuthorization(
                accountSid,
                Environment.GetEnvironmentVariable(AUTHTOKEN)
            );

            fromNumber = Environment.GetEnvironmentVariable(PHONENUMBER);

            var apiClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.twilio.com/2010-04-01"),
                DefaultRequestHeaders = { Authorization = twilioAuthentication.AuthorizationHeader }
            };
            api = RestService.For<ITwilioRestApi>(apiClient);
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
                testOutput.WriteLine($"accountSid = #{accountSid}");
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
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
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
            var message = (exception as ApiException).Content;
            Assert.DoesNotContain("21606", message);
            testOutput.WriteLine(message);
        }

        [Theory]
        [InlineData("+15005550001")]
        [InlineData("+15005550007")]
        [InlineData("+15005550008")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
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
            var message = (exception as ApiException).Content;
            if(!value.Equals("+15005550007"))
                Assert.DoesNotContain("21606", message);
            testOutput.WriteLine(message);
        }

        [Fact]
        public async Task PurchaseAvailableIncomingPhoneNumber()
        {
            var parameters = new Dictionary<string, string>
            {
                {"PhoneNumber", "+15005550006"}
            };
            dynamic response = await api.PurchasePhoneNumber(accountSid, parameters);

            Assert.Equal(accountSid, (string)response.account_sid);
            Assert.Equal("+15005550006", (string)response.phone_number);
        }

        [Theory]
        [InlineData("+15005550000")]
        [InlineData("+15005550001")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public async Task PurchaseUnavalbleIncomingPhoneNumber(string value)
        {
            var parameters = new Dictionary<string, string>
            {
                {"PhoneNumber", value}
            };
            var exception = await Record.ExceptionAsync(() => api.PurchasePhoneNumber(accountSid, parameters));
            Assert.NotNull(exception);
            Assert.IsType<ApiException>(exception);
            var message = (exception as ApiException).Content;
            Assert.DoesNotContain("21606", message);
            testOutput.WriteLine(message);
        }

        [Fact]
        public async Task PurchaseIncomingPhoneNumberUsingAreaCodeWithNumbers()
        {
            var parameters = new Dictionary<string, string>
            {
                {"AreaCode", "500"}
            };
            dynamic response = await api.PurchasePhoneNumber(accountSid, parameters);

            Assert.Equal(accountSid, (string)response.account_sid);
            Assert.Equal("+15005550006", (string)response.phone_number);
        }

        [Fact]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public async Task PurchaseIncomingPhoneNumberUsingAreaCodeWithoutNumbers()
        {
            var parameters = new Dictionary<string, string>
            {
                {"AreaCode", "533"}
            };
            var exception = await Record.ExceptionAsync(() => api.PurchasePhoneNumber(accountSid, parameters));
            Assert.NotNull(exception);
            Assert.IsType<ApiException>(exception);
            var message = (exception as ApiException).Content;
            Assert.Contains("21452", message);
            testOutput.WriteLine(message);
        }
    }
}