namespace Twilio
{
    public class AvailableNumberFilter
    {
        public string AreaCode { get; set; }
        public string Contains { get; set; }
        public bool? SmsEnabled { get; set; }
        public bool? MmsEnabled { get; set; }
        public bool? VoiceEnabled { get; set; }
        public bool? FaxEnabled { get; set; }
        public bool? ExcludeAllAddressRequired { get; set; }
        public bool? ExcludeLocalAddressRequired { get; set; }
        public bool? ExcludeForeignAddressRequired { get; set; }
        public bool? Beta { get; set; }
        public string NearNumber { get; set; }
        public string NearLatLong { get; set; }
        public int? Distance { get; set; }
        public string InPostalCode { get; set; }
        public string InLocality { get; set; }
        public string InRegion { get; set; }
        public string InRateCenter { get; set; }
        public string InLata { get; set; }
    }
}