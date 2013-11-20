using System;

namespace Glimpse.WindowsAzure.Infrastructure
{
    public class WindowsAzureWebsitesEnvironment
        : IWindowsAzureEnvironment
    {
        public bool IsAvailable
        {
            get
            {
                return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
            }
        }

        public string ComputeMode
        {
            get
            {
                VerifyAvailable();
                return Environment.GetEnvironmentVariable("WEBSITE_COMPUTE_MODE");
            }
        }

        public string SiteMode
        {
            get
            {
                VerifyAvailable();
                return Environment.GetEnvironmentVariable("WEBSITE_SITE_MODE");
            }
        }

        public string SiteName
        {
            get
            {
                VerifyAvailable();
                return Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
            }
        }

        public string RemoteConsole
        {
            get
            {   
                VerifyAvailable();
                return string.Format("!<a href=\"https://{0}.scm.azurewebsites.net/DebugConsole/\">launch</a>!", Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
            }
        }

        protected void VerifyAvailable()
        {
            if (!IsAvailable)
            {
                // todo: make this a custom exception type
                throw new Exception("Environment is not available.");
            }
        }
    }
}