using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Refit;

namespace Twilio
{
    public interface ITwilioMessagingApi
    {
        // Base Uri - https://api.twilio.com

        [Post("/2010-04-01/Accounts/{AccountSid}/Messages")]
        Task<JObject> SendSMS(string AccountSid, string To, string From, string Message);
    }
}