using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Refit;

namespace Twilio
{
    public interface ITwilioMessagingApi
    {
        // Base Uri - https://api.twilio.com

        [Post("/2010-04-01/Accounts/{AccountSid}/Messages.json")]
        Task<JObject> SendSMS(string AccountSid, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string,string> parameters);
    }
}