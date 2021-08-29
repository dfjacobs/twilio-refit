using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Refit;

namespace Twilio
{
    public interface ITwilioFaxApi
    {
        // Base Uri - https://fax.twilio.com/v1

        [Get("/Faxes.json")]
        Task<JsonElement> GetFaxes(FaxFilter parameters);

        [Post("/Faxes.json")]
        Task<JsonElement> SendFax([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Get("/Faxes/{FaxSid}.json")]
        Task<JsonElement> GatFax(string FaxSid);

        [Post("/Faxes/{FaxSid}.json")]
        Task<JsonElement> UpdateFax(string FaxSid,
                                [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Delete("/Faxes/{FaxSid}")]
        Task DeleteFax(string FaxSid);

        [Get("/Faxes/{FaxSid}/Media.json")]
        Task<JsonElement> ListMedia(string FaxSid);

        [Get("/Faxes/{FaxSid}/Media/{MediaSid}.json")]
        Task<JsonElement> GetMedia(string FaxSid, string MediaSid);

        [Delete("/Faxes/{FaxSid}/Media/{MediaSid}")]
        Task DeleteMedia(string FaxSid, string MediaSid);
    }
}