using System;

namespace Glimpse.AspNet.Model
{
    public class ConfigurationAuthenticationFormsModel
    {
        public string Cookieless { get; set; }

        public ConfigurationAuthenticationFormsCredentialsModel Credentials { get; set; }

        public string DefaultUrl { get; set; }

        public string Domain { get; set; }

        public bool EnableCrossAppRedirects { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Protection { get; set; }

        public bool RequireSSL { get; set; }

        public bool SlidingExpiration { get; set; }

        public string TicketCompatibilityMode { get; set; }

        public TimeSpan Timeout { get; set; }
    }
}