using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using Glimpse.Net.Extensibility;
using Enviro = System.Environment;

namespace Glimpse.Net.Plugin.ASP
{
    [GlimpsePlugin]
    internal class Environment : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Environment"; }
        }

        public object GetData(HttpApplication application)
        {
            //environment should not change from request to request on a given machine.  We can cache our results in the application store
            var result = application.Application[GlimpseConstants.PluginEnvironmentStore] as IDictionary<string, object>;
            if (result != null) return result;

            var serverSoftware = application.Context.Request.ServerVariables["SERVER_SOFTWARE"];
            var processName = Process.GetCurrentProcess().MainModule.ModuleName;

            //build assemblies table
            var headers = new[] {"Name", "Version", "Culture", "From GAC", "Full Trust"};
            var sysList = new List<object[]>{ headers };
            var appList = new List<object[]>{ headers };

            var allAssemblies = BuildManager.GetReferencedAssemblies().OfType<Assembly>().Concat(AppDomain.CurrentDomain.GetAssemblies()).Distinct().OrderBy(o => o.FullName);

            var sysAssemblies = from a in allAssemblies where a.FullName.StartsWith("System") || a.FullName.StartsWith("Microsoft") select a;
            var appAssemblies = from a in allAssemblies where !a.FullName.StartsWith("System") && !a.FullName.StartsWith("Microsoft") select a;

            foreach (var assembly in sysAssemblies)
            {
                Add(assembly, to:sysList);
            }

            foreach (var assembly in appAssemblies)
            {
                Add(assembly, to:appList);
            }

            result = new Dictionary<string, object>
                              {
                                  {"Machine Name", string.Format("{0} ({1} processors)", Enviro.MachineName, Enviro.ProcessorCount)},
                                  {"Booted", DateTime.Now.AddMilliseconds(Enviro.TickCount*-1)},
                                  {"Operating System", string.Format("{0} ({1} bit)", Enviro.OSVersion.VersionString, Enviro.Is64BitOperatingSystem ? "64" : "32")},
                                  {".NET Framework", string.Format(".NET {0} ({1} bit)", Enviro.Version, IntPtr.Size*8)},
                                  {"Web Server", !string.IsNullOrEmpty(serverSoftware) ? serverSoftware : processName.StartsWith("WebDev.WebServer", StringComparison.InvariantCultureIgnoreCase) ? "Visual Studio Web Development Server" : "Unknown"},
                                  {"Integrated Pipeline", HttpRuntime.UsingIntegratedPipeline.ToString()},
                                  {"Worker Process", processName},
                                  {"Current Trust Level", GetCurrentTrustLevel().ToString()},
                                  {"Application Assemblies", appList},
                                  {"System Assemblies", sysList}
                              };

            application.Application[GlimpseConstants.PluginEnvironmentStore] = result;
            return result;
        }

        public void SetupInit(HttpApplication application)
        {
            throw new NotImplementedException();
        }

        private static void Add(Assembly assembly, ICollection<object[]> to)
        {
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version.ToString();
            var culture = string.IsNullOrEmpty(assemblyName.CultureInfo.Name) ? "_neutral_":assemblyName.CultureInfo.Name;

            to.Add(new [] { assemblyName.Name, version, culture, assembly.GlobalAssemblyCache.ToString(), assembly.IsFullyTrusted.ToString() });
        }
         
        private AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            var levels = new [] {
                                 AspNetHostingPermissionLevel.Unrestricted,
                                 AspNetHostingPermissionLevel.High,
                                 AspNetHostingPermissionLevel.Medium,
                                 AspNetHostingPermissionLevel.Low,
                                 AspNetHostingPermissionLevel.Minimal
                             };

            foreach (var trustLevel in levels)
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                }
                catch (System.Security.SecurityException)
                {
                    continue;
                }

                return trustLevel;
            }

            return AspNetHostingPermissionLevel.None;
        }
    }
}