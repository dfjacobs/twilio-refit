using System;

namespace Twilio
{
    public class FaxFilter
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DateCreatedOnOrBefore { get; set; }
        public DateTime DateCreatedAfter { get; set; }
    }
}