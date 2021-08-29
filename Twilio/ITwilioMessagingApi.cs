using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Refit;

namespace Twilio
{
    public interface ITwilioMessagingApi
    {
        // Base Uri - https://api.twilio.com

        [Post("/2010-04-01/Accounts/{AccountSid}/Messages.json")]
        Task<JsonElement> SendSMS(string AccountSid, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string,string> parameters);
    }
}