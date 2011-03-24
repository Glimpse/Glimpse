using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Web;
using System.Web.Compilation;
using Glimpse.Protocol;
using Enviro = System.Environment;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin]
    public class Environment : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Environment"; }
        }

        public object GetData(HttpApplication application)
        {
            var serverSoftware = application.Context.Request.ServerVariables["SERVER_SOFTWARE"];
            var processName = Process.GetCurrentProcess().MainModule.ModuleName;

            //build assemblies table
            var assemblyList = new List<object[]>
                                   {
                                       new []{"Full Name", "From GAC", "Full Trust"}
                                   };

            var allAssemblies = BuildManager.GetReferencedAssemblies().OfType<Assembly>().Concat(AppDomain.CurrentDomain.GetAssemblies()).Distinct().OrderBy(o => o.FullName);
            foreach (Assembly assembly in allAssemblies)
            {
                assemblyList.Add(new object[] { assembly.FullName, assembly.GlobalAssemblyCache, assembly.IsFullyTrusted });
            }

            return new Dictionary<string, object>
                              {
                                  {"Machine Name", string.Format("{0} ({1} processors)", Enviro.MachineName, Enviro.ProcessorCount)},
                                  {"Booted", DateTime.Now.AddMilliseconds(Enviro.TickCount*-1)},
                                  {"Operating System", string.Format("{0} ({1} bit)", Enviro.OSVersion.VersionString, Enviro.Is64BitOperatingSystem ? "64" : "32")},
                                  {".NET Framework", string.Format(".NET {0} ({1} bit)", Enviro.Version, IntPtr.Size*8)},
                                  {"Web Server", !string.IsNullOrEmpty(serverSoftware) ? serverSoftware : processName.StartsWith("WebDev.WebServer", StringComparison.InvariantCultureIgnoreCase) ? "Visual Studio Web Development Server" : "Unknown"},
                                  {"Integrated Pipeline", HttpRuntime.UsingIntegratedPipeline},
                                  {"Worker Process", processName},
                                  {"Assemblies", assemblyList}
                              };
        }
    }
}