using System;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Twilio.Tests
{
    public class AuthorizationUnitTest
    {
        private readonly string accountSid = "ACxxxxxxxxxxxxxx";
        private readonly string authToken = "TestAuthTokenValue";

        [Fact]
        public void CreatesBasicAuthenticationHeader()
        {
            var authObject = new TwilioAuthorization(accountSid, authToken);
            var header = authObject.AuthorizationHeader;

            Assert.NotNull(header);
            Assert.IsType<AuthenticationHeaderValue>(header);
            Assert.Equal("basic", header.Scheme);
            var expectedValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{accountSid}:{authToken}"));
            Assert.Equal(expectedValue, header.Parameter);
        }

        [Fact]
        public void ThrowsExceptionIfNoAccountSid()
        {
            Exception ex = Assert.Throws<ArgumentNullException>(() => new TwilioAuthorization(null, authToken));
            Assert.Contains("accountSid", ex.Message);
        }

        [Fact]
        public void ThrowsExceptionIfNoAuthToken()
        {
            Exception ex = Assert.Throws<ArgumentNullException>(() => new TwilioAuthorization(accountSid, null));
            Assert.Contains("authToken", ex.Message);
        }
    }
}