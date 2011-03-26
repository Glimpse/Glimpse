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
            var headers = new[] {"Name", "Version", "Culture", "From GAC", "Full Trust"};
            var sysList = new List<object[]>{ headers };
            var appList = new List<object[]>{ headers };

            var allAssemblies = BuildManager.GetReferencedAssemblies().OfType<Assembly>().Concat(AppDomain.CurrentDomain.GetAssemblies()).Distinct().OrderBy(o => o.FullName);

            var sysAssemblies = from a in allAssemblies where a.FullName.StartsWith("System") || a.FullName.StartsWith("Microsoft") select a;
            var appAssemblies = from a in allAssemblies where !a.FullName.StartsWith("System") && !a.FullName.StartsWith("Microsoft") select a;

            foreach (Assembly assembly in sysAssemblies)
            {
                Add(assembly, to:sysList);
            }

            foreach (Assembly assembly in appAssemblies)
            {
                Add(assembly, to:appList);
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
                                  {"Application Assemblies", appList},
                                  {"System Assemblies", sysList}
                              };
        }

        private void Add(Assembly assembly, List<object[]> to)
        {
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version.ToString();
            var culture = string.IsNullOrEmpty(assemblyName.CultureInfo.Name) ? "_neutral_":assemblyName.CultureInfo.Name;

            to.Add(new object[] { assemblyName.Name, version, culture, assembly.GlobalAssemblyCache, assembly.IsFullyTrusted });
        }
    }
}