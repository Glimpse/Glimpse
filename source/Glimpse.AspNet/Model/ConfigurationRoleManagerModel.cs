using System;
using System.Collections.Generic;

namespace Glimpse.AspNet.Model
{
    public class ConfigurationRoleManagerModel
    {
        public bool CookieRequireSSL { get; set; }

        public bool CacheRolesInCookie { get; set; }

        public string CookieName { get; set; }

        public string CookiePath { get; set; }

        public string CookieProtection { get; set; }

        public bool CookieSlidingExpiration { get; set; }

        public TimeSpan CookieTimeout { get; set; }

        public bool CreatePersistentCookie { get; set; }

        public string DefaultProvider { get; set; }

        public string Domain { get; set; }

        public bool Enabled { get; set; }

        public int MaxCachedResults { get; set; }

        public List<ConfigurationRoleManagerProviderSettingsModel> Providers { get; set; }
    }
}