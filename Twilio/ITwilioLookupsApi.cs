using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Refit;

namespace Twilio
{
    public interface ITwilioLookupsApi
    {
        // Base Uri - https://lookups.twilio.com/v1

        [Get("/PhoneNumbers/{PhoneNumber}")]
        Task<JObject> NumberInfo(string PhoneNumber, string CountryCode, string Type);
    }
}