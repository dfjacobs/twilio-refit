using System.Text.Json;
using System.Threading.Tasks;
using Refit;

namespace Twilio
{
    public interface ITwilioLookupsApi
    {
        // Base Uri - https://lookups.twilio.com/v1

        [Get("/PhoneNumbers/{PhoneNumber}")]
        Task<JsonElement> NumberInfo(string PhoneNumber, string CountryCode, string Type);
    }
}