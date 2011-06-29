using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin]
    internal class Environment : IGlimpsePlugin, IProvideGlimpseHelp
    {
        private const string PluginEnvironmentStoreKey = "Glimpse.Plugin.Environment.Store";
        private GlimpseConfiguration Configuration { get; set; }

        [ImportingConstructor]
        public Environment(GlimpseConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Name
        {
            get { return "Environment"; }
        }

        public object GetData(HttpContextBase context)
        {
            //environment should not change from request to request on a given machine.  We can cache our results in the application store
            var cachedData = context.Application[PluginEnvironmentStoreKey] as IDictionary<string, object>;
            if (cachedData != null) return cachedData;

            var environmentName = "_Configure in web.config glimpse/environments_";
            var currentEnviro = Configuration.Environments.GetCurrent(context.Request.Url);

            if (currentEnviro != null)
            {
                environmentName = currentEnviro.Name;
            }

            var serverSoftware = context.Request.ServerVariables["SERVER_SOFTWARE"];
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
             
            cachedData = new Dictionary<string, object>
                              {
                                  {"Environment Name", environmentName},
                                  {"Machine Name", string.Format("{0} ({1} processors)", System.Environment.MachineName, System.Environment.ProcessorCount)},
                                  {"Booted", DateTime.Now.AddMilliseconds(System.Environment.TickCount*-1)},
                                  {"Operating System", string.Format("{0} ({1} bit)", System.Environment.OSVersion.VersionString, System.Environment.Is64BitOperatingSystem ? "64" : "32")},
                                  {".NET Framework", string.Format(".NET {0} ({1} bit)", System.Environment.Version, IntPtr.Size*8)},
                                  {"Web Server", !string.IsNullOrEmpty(serverSoftware) ? serverSoftware : processName.StartsWith("WebDev.WebServer", StringComparison.InvariantCultureIgnoreCase) ? "Visual Studio Web Development Server" : "Unknown"},
                                  {"Integrated Pipeline", HttpRuntime.UsingIntegratedPipeline.ToString()},
                                  {"Debugging", IsInDebug(context)},
                                  {"Current Trust Level", GetCurrentTrustLevel().ToString()},
                                  {"Process", ProcessDetails()},
                                  {"Timezone", TimezoneDetails()},
                                  {"Application Assemblies", appList},
                                  {"System Assemblies", sysList}
                              };

            context.Application[PluginEnvironmentStoreKey] = cachedData;
            return cachedData;
        }

        public void SetupInit()
        {
        }

        private static void Add(Assembly assembly, ICollection<object[]> to)
        {
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version.ToString();
            var culture = string.IsNullOrEmpty(assemblyName.CultureInfo.Name) ? "_neutral_":assemblyName.CultureInfo.Name;

            to.Add(new [] { assemblyName.Name, version, culture, assembly.GlobalAssemblyCache.ToString(), assembly.IsFullyTrusted.ToString() });
        }
         
        private static AspNetHostingPermissionLevel GetCurrentTrustLevel()
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

        private static object IsInDebug(HttpContextBase context)
        {
            var isLocal = context.Request.Url.IsLoopback;
            var isDebug = context.IsDebuggingEnabled;
            if (!isLocal && context.IsDebuggingEnabled)
                return String.Format("*{0}*", isDebug.ToString());
            return isDebug.ToString();
        }

        private static object TimezoneDetails()
        { 
            // get a local time zone info
            var timeZoneInfo = TimeZoneInfo.Local;

            // get it in hours
            var offset = timeZoneInfo.BaseUtcOffset.Hours;

            // add one hour if we are in daylight savings
            var isDaylightSavingTime = false;
            if (timeZoneInfo.IsDaylightSavingTime(DateTime.Now))
            { 
                offset++;
                isDaylightSavingTime = true;
            }

            return new List<object[]>
                           {
                               new object[] { "Current", "Is Daylight Saving", "UtcOffset w/DLS" },
                               new object[] { timeZoneInfo.DisplayName, isDaylightSavingTime.ToString(), offset }
                           }; 
        }

        private static object ProcessDetails()
        {
            var process = Process.GetCurrentProcess();
             
            var processName = process.MainModule.ModuleName;
            var startTime = process.StartTime;
            var uptimeSpan = DateTime.Now.Subtract(startTime);

            var uptime = "";
            if (uptimeSpan.Days > 0) 
                uptime = uptimeSpan.Days + " days"; 
            if (uptimeSpan.Hours > 0) 
                uptime += uptimeSpan.Hours + " hrs"; 
            uptime += uptimeSpan.Minutes + " min";	

            return new List<object[]>
                           {
                               new object[] { "Worker Process", "Process ID", "Start Time", "Uptime" },
                               new object[] { processName, process.Id, String.Format("{0} {1}", startTime.ToShortDateString(), startTime.ToLongTimeString()), uptime }
                           }; 
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Environment"; }
        }
    }
}