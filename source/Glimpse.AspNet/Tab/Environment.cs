using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Environment : AspNetTab, IDocumentation, IKey
    { 
        private readonly IEnumerable<string> systemNamspaces = new List<string> { "System", "Microsoft" }; 

        public override string Name
        {
            get { return "Environment"; }
        }

        public string Key 
        {
            get { return "glimpse_environment"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Plugin/Environment"; }
        }

        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();

            var result = httpContext.Application["Glimpse.AspNet.Environment"] as EnvironmentModel;
            if (result == null)
            {
                result = new EnvironmentModel
                    {
                        WebServer = BuildWebServerDetails(httpContext),
                        Framework = BuildFrameworkDetails(httpContext),
                        Machine = BuildMachineDetails(),
                        TimeZone = BuildTimeZoneDetails(),
                        Process = BuildProcessDetails()
                    };
                FindAssemblies(result);

                httpContext.Application["Glimpse.AspNet.Environment"] = result;
            }

            return result;
        }

        protected virtual IEnumerable<Assembly> FindAllAssemblies()
        {
            return BuildManager.GetReferencedAssemblies().OfType<Assembly>().Concat(AppDomain.CurrentDomain.GetAssemblies()).Distinct().OrderBy(o => o.FullName);
        }

        private static EnvironmentWebServerModel BuildWebServerDetails(HttpContextBase context)
        {
            var serverSoftware = context.Request.ServerVariables["SERVER_SOFTWARE"];
            var processName = Process.GetCurrentProcess().MainModule.ModuleName;

            var serverType = !string.IsNullOrEmpty(serverSoftware) ? serverSoftware : processName.StartsWith("WebDev.WebServer", StringComparison.InvariantCultureIgnoreCase) ? "Visual Studio Web Development Server" : "Unknown";
            var integratedPipeline = HttpRuntime.UsingIntegratedPipeline;

            return new EnvironmentWebServerModel { ServerType = serverType, IntegratedPipeline = integratedPipeline };
        }

        private EnvironmentFrameworkModel BuildFrameworkDetails(HttpContextBase context)
        {
            var dotnetFramework = string.Format(".NET {0} ({1} bit)", System.Environment.Version, IntPtr.Size * 8);
            var debugging = context.IsDebuggingEnabled;
            var serverCulture = Thread.CurrentThread.CurrentCulture.DisplayName;
            var currentTrustLevel = GetCurrentTrustLevel().ToString();

            return new EnvironmentFrameworkModel { DotnetFramework = dotnetFramework, Debugging = debugging, ServerCulture = serverCulture, CurrentTrustLevel = currentTrustLevel };
        }

        private EnvironmentMachineModel BuildMachineDetails()
        {
            var is64BitOperatingSystem = Is64BitOperatingSystem();
            var name = string.Format("{0} ({1} processors)", System.Environment.MachineName, System.Environment.ProcessorCount);
            var operatingSystem = string.Format("{0} ({1} bit)", System.Environment.OSVersion.VersionString, is64BitOperatingSystem == null ? "?" : is64BitOperatingSystem.Value ? "64" : "32");  
            var startTime = DateTime.Now.AddMilliseconds(System.Environment.TickCount * -1);

            return new EnvironmentMachineModel { Name = name, OperatingSystem = operatingSystem, StartTime = startTime };
        }
        
        private EnvironmentTimeZoneModel BuildTimeZoneDetails()
        { 
            var timeZoneInfo = TimeZoneInfo.Local;

            var name = timeZoneInfo.DaylightName;
            var utcOffset = timeZoneInfo.BaseUtcOffset.Hours;
            var utcOffsetWithDls = timeZoneInfo.BaseUtcOffset.Hours; 
            var isDaylightSavingTime = false;
            if (timeZoneInfo.IsDaylightSavingTime(DateTime.Now))
            {
                utcOffsetWithDls++;
                isDaylightSavingTime = true;
            }

            return new EnvironmentTimeZoneModel { Name = name, IsDaylightSavingTime = isDaylightSavingTime, UtcOffset = utcOffset, UtcOffsetWithDls = utcOffsetWithDls };
        }

        private EnvironmentProcessModel BuildProcessDetails()
        {
            var process = Process.GetCurrentProcess();

            var processId = process.Id;
            var processName = process.MainModule.ModuleName;
            var startTime = process.StartTime;

            return new EnvironmentProcessModel { ProcessId = processId, WorkerProcess = processName, StartTime = startTime };
        }

        private void FindAssemblies(EnvironmentModel model)
        {
            var allAssemblies = FindAllAssemblies();

            var sysAssemblies = new List<EnvironmentAssemblyModel>();
            var appAssemblies = new List<EnvironmentAssemblyModel>();

            foreach (var assembly in allAssemblies)
            { 
                var assemblyName = assembly.GetName();
                var name = assemblyName.Name;
                var version = assemblyName.Version.ToString();
                var culture = string.IsNullOrEmpty(assemblyName.CultureInfo.Name) ? "neutral" : assemblyName.CultureInfo.Name; 
                var fromGac = assembly.GlobalAssemblyCache;
                var fullTrust = IsFullyTrusted(assembly); 

                var result = new EnvironmentAssemblyModel { Name = name, Version = version, Culture = culture, FromGac = fromGac, FullTrust = fullTrust };

                var isSystem = systemNamspaces.Any(systemNamspace => assembly.FullName.StartsWith(systemNamspace)); 
                if (isSystem)
                {
                    sysAssemblies.Add(result);
                }
                else
                {
                    appAssemblies.Add(result);
                } 
            }

            if (appAssemblies.Count > 0)
            {
                model.ApplicationAssemblies = appAssemblies;
            }

            if (sysAssemblies.Count > 0)
            {
                model.SystemAssemblies = sysAssemblies;
            }
        }

        private AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            var levels = new[] { AspNetHostingPermissionLevel.Unrestricted, AspNetHostingPermissionLevel.High, AspNetHostingPermissionLevel.Medium, AspNetHostingPermissionLevel.Low, AspNetHostingPermissionLevel.Minimal };
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

        private bool? Is64BitOperatingSystem()
        {
#if NET35
            return null;      
#else
            return System.Environment.Is64BitOperatingSystem;
#endif
        }

        private bool? IsFullyTrusted(Assembly assembly)
        {
#if NET35
            return null;      
#else
            return assembly.IsFullyTrusted;
#endif
        }
    }
}
