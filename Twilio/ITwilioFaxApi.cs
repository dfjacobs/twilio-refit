using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Refit;

namespace Twilio
{
    public interface ITwilioFaxApi
    {
        // Base Uri - https://fax.twilio.com/v1

        [Get("/Faxes.json")]
        Task<JObject> GetFaxes(FaxFilter parameters);

        [Post("/Faxes.json")]
        Task<JObject> SendFax([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Get("/Faxes/{FaxSid}.json")]
        Task<JObject> GatFax(string FaxSid);

        [Post("/Faxes/{FaxSid}.json")]
        Task<JObject> UpdateFax(string FaxSid,
                                [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Delete("/Faxes/{FaxSid}")]
        Task DeleteFax(string FaxSid);

        [Get("/Faxes/{FaxSid}/Media.json")]
        Task<JObject> ListMedia(string FaxSid);

        [Get("/Faxes/{FaxSid}/Media/{MediaSid}.json")]
        Task<JObject> GetMedia(string FaxSid, string MediaSid);

        [Delete("/Faxes/{FaxSid}/Media/{MediaSid}")]
        Task DeleteMedia(string FaxSid, string MediaSid);
    }
}