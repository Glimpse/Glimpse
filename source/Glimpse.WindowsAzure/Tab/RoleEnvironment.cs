using Glimpse.Core.Extensibility;
using Glimpse.WindowsAzure.Infrastructure;

namespace Glimpse.WindowsAzure.Tab
{
    public class RoleEnvironment
        : TabBase, IKey
    {
        private readonly RoleEnvironmentWrapper roleEnvironment;

        public RoleEnvironment()
        {
            roleEnvironment = new RoleEnvironmentWrapper();
        }

        public override string Name
        {
            get { return "Role Environment"; }
        }

        public string Key 
        {
            get { return "glimpse_waz_roleenvironment"; }
        }

        public override object GetData(ITabContext context)
        {
            if (roleEnvironment.IsAvailable)
            {
                return roleEnvironment;
            }
            return "The application is not running in Windows Azure role environment.";
        }
    }
}