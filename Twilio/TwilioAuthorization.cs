using System;
using System.Text;

namespace Twilio
{
    public class TwilioAuthorization
    {
        public System.Net.Http.Headers.AuthenticationHeaderValue AuthorizationHeader { get; }

        public TwilioAuthorization(string accountSid, string authToken)
        {
            if(string.IsNullOrWhiteSpace(accountSid)) throw new ArgumentNullException(nameof(accountSid));
            if(string.IsNullOrWhiteSpace(authToken)) throw new ArgumentNullException(nameof(authToken));

            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{accountSid}:{authToken}"));
            AuthorizationHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("basic", encoded);
        }
    }
}