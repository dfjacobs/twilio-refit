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

### Changes

Version 4.1 has been updated to Refit 6.3.2. It also removes the obsolete [Twilio Fax API](https://support.twilio.com/hc/en-us/articles/223136667-Fax-Support-on-Twilio).

Version 4 of the project is built on Refit 6. Since Refit 6 defaults to using the [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview) 
JSON serializer introduced in .Net Core 3.1, I decided to migrate the project to the same serializer. This required a change to the API interfaces.
The [Newtonsoft.Json.Linq.JObject](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JObject.htm) class has been replaced with the 
[System.Text.Jsom.JsonElement](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonelement?view=net-5.0) struct. Both classes represent 
generic JSON entities, however JsonElement cannot currently be assigned to a dynamic C# variable. Microsoft may add this capability to System.Text.Json
sometime in the future.

If support for Json.Net is desired, use version 3.1.1 of the project. That version uses Json.Net, and you can use the API interfaces from that version 
with Refit 6. Refit 6 can be configured to use Json.Net in place of System.Text.Json.

Version 4 still supports .Net Standard 2.0 and .Net Framework 4.6.2.

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

### Unit and Integration Tests

The tests included in the project are written to retrieve the Twilio API credentials and phone numbers from the environment.
The [DotNetEnv](https://github.com/tonerdo/dotnet-env) NuGet package is used to load the environment variables from a .env file.

The included interface tests would be considered integration tests instead of unit tests.
When modifying the provided interfaces or adding new ones, you should create tests to verify that the interface is defined correctly.

For integration testing, you should create a new Twilio project at <https://www.twilio.com>.
The project will have the API credentials you need to run the integration tests.
Twilio allows you to create multiple API keys, you should use a different API keys for testing and production.
Twilio projects define phone numbers and credentials for Messaging API testing, see <https://www.twilio.com/console/phone-numbers/runtime/test-credentials>.
Other Twilio APIs will need valid API credentials for any integration tests. 
Create a .env file containing the API keys used for integration testing.
Do not check in any .env files that contain API credentials! You do not want to publish .env files to public repositories! 

The keys that need to be defined in the .env file for running the included integration tests:
```
TWILIO_ACCOUNTSID_TEST=Replace with the project's test AccountSID
TWILIO_AUTHTOKEN_TEST=Replace with the project's test AuthToken
TWILIO_PHONENUMBER=+15005550006
TWILIO_ACCOUNTSID=Replace with a valid AccountSID for testing the Lookup API
TWILIO_AUTHTOKEN=Replace with a valid AuthToken for testing the Lookup API
```  

### Limitations

The project creates libraries for .Net Standard 2.0 and .Net Framework 4.6.2.
 
There is no support for TwiML.  