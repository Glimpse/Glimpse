using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.AspNet.Model
{
    public class EnvironmentModel
    { 
        public EnvironmentWebServerModel WebServer { get; set; }

        public EnvironmentFrameworkModel Framework { get; set; }

        public EnvironmentMachineModel Machine { get; set; }

        public EnvironmentTimeZoneModel TimeZone { get; set; }

        public EnvironmentProcessModel Process { get; set; }

        public IEnumerable<EnvironmentAssemblyModel> ApplicationAssemblies { get; set; }

        public IEnumerable<EnvironmentAssemblyModel> SystemAssemblies { get; set; }
    }
}
