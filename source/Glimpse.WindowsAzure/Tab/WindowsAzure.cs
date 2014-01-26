using Glimpse.Core.Extensibility;
using Glimpse.WindowsAzure.Infrastructure;

namespace Glimpse.WindowsAzure.Tab
{

    public class WindowsAzure
        : TabBase, IKey
    {
        private readonly IWindowsAzureEnvironment windowsAzureEnvironment;

        public WindowsAzure()
        {
            windowsAzureEnvironment = new WindowsAzureWebsitesEnvironment();
            if (!windowsAzureEnvironment.IsAvailable)
            {
                windowsAzureEnvironment = new WindowsAzureCloudServicesEnvironment();
            }
        }

        public override string Name
        {
            get { return "Azure Environment"; }
        }

        public string Key 
        {
            get { return "glimpse_waz_environment"; }
        }

        public override object GetData(ITabContext context)
        {
            if (windowsAzureEnvironment.IsAvailable)
            {
                return windowsAzureEnvironment;
            }
            return "The application is not running in Windows Azure Cloud Services or Windows Azure Websites.";
        }
    }
}