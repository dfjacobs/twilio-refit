using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Refit;

namespace Twilio
{
    public interface ITwilioRestApi
    {
        // Base Uri - https://api.twilio.com/2010-04-01

#region SMS resource
        [Post("/Accounts/{AccountSid}/Messages.json")]
        Task<JsonElement> SendSMS(string AccountSid, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);
#endregion

#region Account resource
        [Get("/Accounts.json")]
        Task<JsonElement> GetAllAccounts(string FriendlyName, string status);

        [Post("/Accounts.json")]
        Task<JsonElement> CreateAccount([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Get("/Accounts/{AccountSid}.json")]
        Task<JsonElement> GetAccount(string AccountSid);

        [Post("/Accounts/{AccountSid}.json")]
        Task<JsonElement> UpdateAccount(string AccountSid, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);
#endregion

#region Available Phone Numbers
        [Get("/Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/Local.json")]
        Task<JsonElement> GetAvailableLocalNumbers(string AccountSid, string IsoCountryCode, AvailableNumberFilter parameters);

        [Get("/Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/TollFree.json")]
        Task<JsonElement> GetAvailableTollFreeNumbers(string AccountSid, string IsoCountryCode, AvailableNumberFilter parameters);

        [Get("/Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/Mobile.json")]
        Task<JsonElement> GetAvailableMobileNumbers(string AccountSid, string IsoCountryCode, AvailableNumberFilter parameters);
#endregion

#region Incoming Phone Numbers

        [Get("/Accounts/{AccountSid}/IncomingPhoneNumbers.json")]
        Task<JsonElement> GetInboundPhoneNumbers(string AccountSid, IncomingNumberFilter parameters);

        [Post("/Accounts/{AccountSid}/IncomingPhoneNumbers.json")]
        Task<JsonElement> PurchasePhoneNumber(string AccountSid,
            [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Get("/Accounts/{AccountSid}/IncomingPhoneNumbers/{IncomingPhoneNumberSid}.json")]
        Task<JsonElement> GetInboundNumber(string AccountSid, string IncomingPhoneNumberSid);

        [Post("/Accounts/{AccountSid}/IncomingPhoneNumbers/{IncomingPhoneNumberSid}.json")]
        Task<JsonElement> UpdateInboundNumber(string AccountSid, string IncomingPhoneNumberSid,
            [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> parameters);

        [Delete("/Accounts/{AccountSid}/IncomingPhoneNumbers/{IncomingPhoneNumberSid}")]
        Task ReleaseInboundNumber(string AccountSid, string IncomingPhoneNumberSid);

#endregion
    }
}