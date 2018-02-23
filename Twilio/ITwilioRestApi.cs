using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Refit;

namespace Twilio
{
    public interface ITwilioRestApi
    {
        // Base Uri - https://api.twilio.com/2010-04-01

        #region SMS resource
        [Post("/Accounts/{AccountSid}/Messages.json")]
        Task<JObject> SendSMS(string AccountSid, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);
        #endregion

        #region Account resource
        [Get("/Accounts.json")]
        Task<JObject> GetAllAccounts(string FriendlyName, string status);

        [Post("/Accounts.json")]
        Task<JObject> CreateAccount([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Get("/Accounts/{AccountSid}.json")]
        Task<JObject> GetAccount(string AccountSid);

        [Post("/Accounts/{AccountSid}.json")]
        Task<JObject> UpdateAccount(string AccountSid, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);
        #endregion

        #region Available Phone Numbers
        [Get("/Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/Local.json")]
        Task<JObject> GetAvailableLocalNumbers(string AccountSid, string IsoCountryCode, AvailableNumberFilter parameters);

        [Get("/Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/TollFree.json")]
        Task<JObject> GetAvailableTollFreeNumbers(string AccountSid, string IsoCountryCode, AvailableNumberFilter parameters);

        [Get("/Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/Mobile.json")]
        Task<JObject> GetAvailableMobileNumbers(string AccountSid, string IsoCountryCode, AvailableNumberFilter parameters);
        #endregion

        #region Incoming Phone Numbers

        [Get("/Accounts/{AccountSid}/IncomingPhoneNumbers.json")]
        Task<JObject> GetInboundPhoneNumbers(string AccountSid, IncomingNumberFilter parameters);

        [Post("/Accounts/{AccountSid}/IncomingPhoneNumbers.json")]
        Task<JObject> PurchasePhoneNumber(string AccountSid,
            [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Get("/Accounts/{AccountSid}/IncomingPhoneNumbers/{IncomingPhoneNumberSid}.json")]
        Task<JObject> GetInboundNumber(string AccountSid, string IncomingPhoneNumberSid);

        [Post("/Accounts/{AccountSid}/IncomingPhoneNumbers/{IncomingPhoneNumberSid}.json")]
        Task<JObject> UpdateInboundNumber(string AccountSid, string IncomingPhoneNumberSid,
            [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Delete("/Accounts/{AccountSid}/IncomingPhoneNumbers/{IncomingPhoneNumberSid}")]
        Task ReleaseInboundNumber(string AccountSid, string IncomingPhoneNumberSid);

        #endregion
    }
}