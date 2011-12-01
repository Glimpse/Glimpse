using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;

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
            var cachedData = context.Application[PluginEnvironmentStoreKey] as GlimpseSection;
            if (cachedData != null) return cachedData;

            var root = new GlimpseSection("Key", "Value");

            var environmentName = "Configure in web.config glimpse/environments";
            var currentEnviro = Configuration.Environments.GetCurrent(context.Request.Url);
            if (currentEnviro != null)
            {
                environmentName = currentEnviro.Name;
            }

            //build assemblies table
            var headers = new[] { "Name", "Version", "Culture", "From GAC", "Full Trust" };
            var appList = new GlimpseSection(headers);
            var sysList = new GlimpseSection(headers);

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

            root.AddRow().Column("Environment Name").Column(environmentName).UnderlineIf(currentEnviro == null);
            root.AddRow().Column("Machine").Column(MachineDetails());
            root.AddRow().Column("Web Server").Column(WebServerDetails(context));
            root.AddRow().Column("Framework").Column(FrameworkDetails(context));
            root.AddRow().Column("Process").Column(ProcessDetails());
            root.AddRow().Column("Timezone").Column(TimezoneDetails());
            root.AddRow().Column("Application Assemblies").Column(appList);
            root.AddRow().Column("Application Assemblies").Column(sysList);

            context.Application[PluginEnvironmentStoreKey] = root;
            return root;
        }

        public void SetupInit()
        {
        }

        private static void Add(Assembly assembly, GlimpseSection to)
        {
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version.ToString();
            var culture = string.IsNullOrEmpty(assemblyName.CultureInfo.Name) ? "neutral".Underline() : assemblyName.CultureInfo.Name;

            to.AddRow()
                .Column(assemblyName.Name)
                .Column(version)
                .Column(culture)
                .Column(assembly.GlobalAssemblyCache.ToString())
                .Column(assembly.IsFullyTrusted.ToString());
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
            
            return isDebug.ToString()
                .BoldIf(!isLocal && context.IsDebuggingEnabled);
        }

        private static GlimpseSection WebServerDetails(HttpContextBase context)
        {
            var serverSoftware = context.Request.ServerVariables["SERVER_SOFTWARE"];
            var processName = Process.GetCurrentProcess().MainModule.ModuleName;

            var serverType = !string.IsNullOrEmpty(serverSoftware)
                ? serverSoftware
                : processName.StartsWith("WebDev.WebServer", StringComparison.InvariantCultureIgnoreCase) ? "Visual Studio Web Development Server" : "Unknown";

            var integratedPipeline = HttpRuntime.UsingIntegratedPipeline.ToString();

            var section = new GlimpseSection("Type", "Integrated Pipeline");
            section.AddRow().Column(serverType).Column(integratedPipeline);
            return section;
        }

        private static GlimpseSection FrameworkDetails(HttpContextBase context)
        {
            var dotnetFramework = string.Format(".NET {0} ({1} bit)", System.Environment.Version, IntPtr.Size * 8);
            var debugging = IsInDebug(context).ToString();
            var serverCulture = Thread.CurrentThread.CurrentCulture;
            var currentTrustLevel = GetCurrentTrustLevel().ToString();

            var section = new GlimpseSection(".NET Framework", "Debugging", "Server Culture", "Current Trust Level");
            section.AddRow().Column(dotnetFramework).Column(debugging).Column(serverCulture).Column(currentTrustLevel);
            return section;
        }

        private static GlimpseSection MachineDetails()
        {
            var name = string.Format("{0} ({1} processors)", System.Environment.MachineName, System.Environment.ProcessorCount);
            var operatingSystem = string.Format("{0} ({1} bit)", System.Environment.OSVersion.VersionString, System.Environment.Is64BitOperatingSystem ? "64" : "32");
            var machineStarttime = DateTime.Now.AddMilliseconds(System.Environment.TickCount * -1);
            var uptime = GetUptime(machineStarttime);
            
            // TODO: Add uptime
            var section = new GlimpseSection("Name", "Operating System", "Start Time");
            section.AddRow().Column(name).Column(operatingSystem).Column(machineStarttime);
            return section;
        }

        private static GlimpseSection TimezoneDetails()
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

            var section = new GlimpseSection("Current", "Is Daylight Saving", "UtcOffset w/DLS");
            section.AddRow().Column(timeZoneInfo.DisplayName).Column(isDaylightSavingTime.ToString()).Column(offset);
            return section;
        }

        private static GlimpseSection ProcessDetails()
        {
            var process = Process.GetCurrentProcess();
             
            var processName = process.MainModule.ModuleName;
            var startTime = process.StartTime; 
            var uptime = GetUptime(startTime);

            // TODO: Add uptime
            var section = new GlimpseSection("Worker Process", "Process ID", "Start Time");
            section.AddRow().Column(processName).Column(process.Id).Column(startTime);
            return section;
        }

        private static string GetUptime(DateTime startTime)
        {
            var uptimeSpan = DateTime.Now.Subtract(startTime);

            var uptime = "";
            if (uptimeSpan.Days > 0)
                uptime = uptimeSpan.Days + " days ";
            if (uptimeSpan.Hours > 0)
                uptime += uptimeSpan.Hours + " hrs ";
            uptime += uptimeSpan.Minutes + " min";

            return uptime;
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Environment"; }
        }
    }
}