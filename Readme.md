## Twilio.Refit: Twilio API library implementation based on Refit

This project supports calling the [Twilio](https://www.twilio.com) API with the [Refit](https://reactiveui.github.io/refit/) library.

### Rationale

Unlike the official [Twilio .Net SDK](https://www.twilio.com/docs/libraries/csharp-dotnet), this project does not implement all of the Twilio APIs.
It is a thin wrapper that enables making REST requests to the individual services offered by Twilio.

Each Twilio service has a corresponding interface. This interface only needs to be changed if the underlying REST API changes.
The official Twilio SDK implements all of the APIs, and therefore gets updated frequently (several times a month).
I found that this adds code bloat, and makes keeping the external nuget packages up to date difficult.

The Refit implementation also makes testing a lot simpler.  Each Twilio service is represented by an interface.
These interfaces make it easy to replace calls to Twilio with calls to a test component.

### Usage

The project is made up of interfaces representing each Twilio API endpoint, and a class that creates the basic authentication header required to make a REST request to Twilio.

A typical application will use the [Twilio REST API](https://www.twilio.com/docs/iam/api) to access the associated Twilio account,
and one or more Twilio APIs to access the various services. Each service has a base url and one or more REST resources.
The project defines an Refit interface for each one of the services. To build an application, you create an object that creates the Twilio
authentication header and creates Refit implementations of each of the API interfaces that you will use.

```csharp
    public class TwilioApi
    {
        // Environment variables
        private const string ACCOUNTSID = "TWILIO_ACCOUNTSID";
        private const string AUTHTOKEN = "TWILIO_AUTHTOKEN";
        private const string PHONENUMBER = "TWILIO_PHONENUMBER";

        // Refit API interfaces
        public ITwilioMessagingApi MessagingApi;
        public ITwilioLookupsApi LookupsApi;

        // Twilio properties
        public string AccountSid;
        public string SmsNumber;

        // Twilio configuration
        private const string MESSAGINGURL = "https://api.twilio.com";
        private const string LOOKUPSURL = "https://lookups.twilio.com/v1";

        public TwilioApi()
        {
            var accountSid = Environment.GetEnvironmentVariable(ACCOUNTSID);
            var authToken = Environment.GetEnvironmentVariable(AUTHTOKEN);
            var phoneNumber = Environment.GetEnvironmentVariable(PHONENUMBER);

            if (string.IsNullOrWhiteSpace(accountSid) ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(authToken))
            {
                throw new Exception("Environment variables not set");
            }

            var twilioAuthentication = new TwilioAuthorization(accountSid, authToken);

            var messagingClient = new HttpClient
            {
                BaseAddress = new Uri(MESSAGINGURL),
                DefaultRequestHeaders = { Authorization = twilioAuthentication.AuthorizationHeader }
            };
            MessagingApi = RestService.For<ITwilioMessagingApi>(messagingClient);

            var lookupsClient = new HttpClient
            {
                BaseAddress = new Uri(LOOKUPSURL),
                DefaultRequestHeaders = { Authorization = twilioAuthentication.AuthorizationHeader }
            };
            LookupsApi = RestService.For<ITwilioLookupsApi>(lookupsClient);

            AccountSid = accountSid;
            SmsNumber = phoneNumber;
        }

        public TwilioApi(ITwilioLookupsApi lookupsApi, ITwilioMessagingApi messagingApi,
            string accountSid, string smsNumber)
        {
            LookupsApi = lookupsApi ?? throw new ArgumentNullException(nameof(lookupsApi));
            MessagingApi = messagingApi ?? throw new ArgumentNullException(nameof(messagingApi));
            AccountSid = accountSid ?? throw new ArgumentNullException(nameof(accountSid));
            SmsNumber = smsNumber ?? throw new ArgumentNullException(nameof(smsNumber));
        }
    }
```
