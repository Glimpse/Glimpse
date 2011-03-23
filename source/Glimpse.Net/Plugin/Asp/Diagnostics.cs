using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Web;
using System.Web.Compilation;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin]
    public class Diagnostics : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Diagnostics"; }
        }

        public object GetData(HttpApplication application)
        {
            var diagnostics = new DiagnosticsResults();

            var results = new Dictionary<string, object>();
            results.Add("Operating System", diagnostics.EnvironmentInformation.OperatingSystem);
            results.Add(".NET Framework", diagnostics.EnvironmentInformation.NetFrameworkVersion);
            results.Add(".NET Bitness", diagnostics.EnvironmentInformation.NetFrameworkBitness);
            results.Add("Web Server", diagnostics.EnvironmentInformation.ServerSoftware);
            results.Add("Integrated Pipeline", diagnostics.EnvironmentInformation.IsIntegrated);
            results.Add("Worker Process", diagnostics.EnvironmentInformation.WorkerProcess);
            results.Add("Homogenous App Domain", IsAppDomainHomogenous(AppDomain.CurrentDomain));
            results.Add("FullTrust", IsAssemblyFullTrust(GetType().Assembly));



                    <% if (results.LoadedMvcCoreAssemblies.Length == 0) { %><p class="error">An ASP.NET MVC assembly has not been loaded into this application.</p><% } %>
        <% if (results.LoadedMvcCoreAssemblies.Length > 1) { %><p class="error">Multiple ASP.NET MVC assemblies have been loaded into this application.</p><% } %>
        <% foreach (var info in results.LoadedMvcCoreAssemblies) { outputAssemblyInfo(info); } %>



            return results;
        }

        private static string IsAppDomainHomogenous(AppDomain appDomain)
        {
            // AppDomain.IsHomogenous didn't exist prior to .NET 4, so use Reflection to look it up
            PropertyInfo pInfo = typeof(AppDomain).GetProperty("IsHomogenous");
            if (pInfo == null)
            {
                return "unknown";
            }

            // MethodInfo.Invoke demands ReflectionPermission when the target is AppDomain, but since target method is transparent we can instantiate a Delegate instead
            return Convert.ToString(((Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), appDomain, pInfo.GetGetMethod()))());
        }

        private static string IsAssemblyFullTrust(Assembly assembly)
        {
            // Assembly.IsFullyTrusted didn't exist prior to .NET 4, so use Reflection to look it up
            PropertyInfo pInfo = typeof(Assembly).GetProperty("IsFullyTrusted");
            if (pInfo == null)
            {
                return "unknown";
            }

            // MethodInfo.Invoke demands ReflectionPermission when the target is Assembly, but since target method is transparent we can instantiate a Delegate instead
            return Convert.ToString(((Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), assembly, pInfo.GetGetMethod()))());
        }
    }

    public class DiagnosticsResults
    {
        private static readonly DateTime _utilityDate = new DateTime(2010, 12, 10);
        private static readonly string _utilityVersion = "v11";

        private static readonly MvcCoreAssemblyInfo[] _mvcCoreAssemblyHistory = new MvcCoreAssemblyInfo[] {
        // v1
        new MvcCoreAssemblyInfo() { Version = "1.0.30218.0", Name = "ASP.NET MVC 1.0 Preview 2" },
        new MvcCoreAssemblyInfo() { Version = "1.0.30508.0", Name = "ASP.NET MVC 1.0 Preview 3" },
        new MvcCoreAssemblyInfo() { Version = "1.0.30714.0", Name = "ASP.NET MVC 1.0 Preview 4" },
        new MvcCoreAssemblyInfo() { Version = "1.0.30826.0", Name = "ASP.NET MVC 1.0 Preview 5" },
        new MvcCoreAssemblyInfo() { Version = "1.0.31003.0", Name = "ASP.NET MVC 1.0 Beta" },
        new MvcCoreAssemblyInfo() { Version = "1.0.40112.0", Name = "ASP.NET MVC 1.0 RC 1" },
        new MvcCoreAssemblyInfo() { Version = "1.0.40128.0", Name = "ASP.NET MVC 1.0 RC 1 Refresh" },
        new MvcCoreAssemblyInfo() { Version = "1.0.40216.0", Name = "ASP.NET MVC 1.0 RC 2" },
        new MvcCoreAssemblyInfo() { Version = "1.0.40310.0", Name = "ASP.NET MVC 1.0 RTM", FuturesVersion = "1.0.40310.0" },
        
        // v2
        new MvcCoreAssemblyInfo() { Version = "1.1.40430.0", Name = "ASP.NET MVC 1.1 Preview 0" },
        new MvcCoreAssemblyInfo() { Version = "2.0.40724.0", Name = "ASP.NET MVC 2 Preview 1", FuturesVersion = "2.0.40724.0" },
        new MvcCoreAssemblyInfo() { Version = "2.0.41001.0", Name = "ASP.NET MVC 2 Preview 2", FuturesVersion = "2.0.41001.0" },
        new MvcCoreAssemblyInfo() { Version = "2.0.41116.0", Name = "ASP.NET MVC 2 Beta", FuturesVersion = "2.0.41116.0" },
        new MvcCoreAssemblyInfo() { Version = "2.0.41211.0", Name = "ASP.NET MVC 2 RC 1", FuturesVersion = "2.0.41211.0" },
        new MvcCoreAssemblyInfo() { Version = "2.0.50129.0", Name = "ASP.NET MVC 2 RC 2", FuturesVersion = "2.0.50129.0" },
        new MvcCoreAssemblyInfo() { Version = "2.0.50217.0", Name = "ASP.NET MVC 2 RTM", FuturesVersion = "2.0.50217.0" },
        
        // v3
        new MvcCoreAssemblyInfo() { Version = "3.0.10714.0", Name = "ASP.NET MVC 3 Preview 1", FuturesVersion = "3.0.10714.0" },
        new MvcCoreAssemblyInfo() { Version = "3.0.10927.0", Name = "ASP.NET MVC 3 Beta", FuturesVersion = "3.0.10927.0" },
        new MvcCoreAssemblyInfo() { Version = "3.0.11029.0", Name = "ASP.NET MVC 3 RC 1", FuturesVersion = "3.0.11029.0" },
        new MvcCoreAssemblyInfo() { Version = "3.0.11209.0", Name = "ASP.NET MVC 3 RC 2", FuturesVersion = "3.0.11209.0" },
    };

        private static readonly MvcFuturesAssemblyInfo[] _mvcFuturesAssemblyHistory = new MvcFuturesAssemblyInfo[] {
        // v1
        new MvcFuturesAssemblyInfo() { Version = "1.0.40310.0", Name = "ASP.NET MVC 1.0 RTM Futures", DownloadUrl = "http://go.microsoft.com/fwlink/?LinkID=198018" },
        
        // v2
        new MvcFuturesAssemblyInfo() { Version = "2.0.40724.0", Name = "ASP.NET MVC 2 Preview 1 Futures" },
        new MvcFuturesAssemblyInfo() { Version = "2.0.41001.0", Name = "ASP.NET MVC 2 Preview 2 Futures" },
        new MvcFuturesAssemblyInfo() { Version = "2.0.41116.0", Name = "ASP.NET MVC 2 Beta Futures" },
        new MvcFuturesAssemblyInfo() { Version = "2.0.41211.0", Name = "ASP.NET MVC 2 RC 1 Futures" },
        new MvcFuturesAssemblyInfo() { Version = "2.0.50129.0", Name = "ASP.NET MVC 2 RC 2 Futures" },
        new MvcFuturesAssemblyInfo() { Version = "2.0.50217.0", Name = "ASP.NET MVC 2 RTM Futures", DownloadUrl = "http://go.microsoft.com/fwlink/?LinkID=183739" },
        
        // v3
        new MvcFuturesAssemblyInfo() { Version = "3.0.10714.0", Name = "ASP.NET MVC 3 Preview 1 Futures" },
        new MvcFuturesAssemblyInfo() { Version = "3.0.10927.0", Name = "ASP.NET MVC 3 Beta Futures" },
        new MvcFuturesAssemblyInfo() { Version = "3.0.11029.0", Name = "ASP.NET MVC 3 RC 1 Futures", DownloadUrl = "http://go.microsoft.com/fwlink/?LinkID=206023" },
        new MvcFuturesAssemblyInfo() { Version = "3.0.11209.0", Name = "ASP.NET MVC 3 RC 2 Futures", DownloadUrl = "http://go.microsoft.com/fwlink/?LinkID=206024" },
    };

        public DiagnosticsResults()
        {
            EnvironmentInformation = GetEnvironmentInformation();
            AllAssemblies = BuildManager.GetReferencedAssemblies().OfType<Assembly>().Concat(AppDomain.CurrentDomain.GetAssemblies()).Distinct().OrderBy(o => o.FullName).ToArray();
            LoadedMvcCoreAssemblies = AllAssemblies.Where(IsMvcAssembly).Select<Assembly, LoadedAssemblyInfo<MvcCoreAssemblyInfo>>(GetMvcAssemblyInformation).ToArray();
            LoadedMvcFuturesAssemblies = AllAssemblies.Where(IsMvcFuturesAssembly).Select<Assembly, LoadedAssemblyInfo<MvcFuturesAssemblyInfo>>(GetFuturesAssemblyInformation).ToArray();

            IsFuturesConflict = (LoadedMvcCoreAssemblies.Length == 1 && LoadedMvcFuturesAssemblies.Length == 1 &&
                                 LoadedMvcCoreAssemblies[0].MvcAssemblyInfo.FuturesVersion !=
                                 LoadedMvcFuturesAssemblies[0].MvcAssemblyInfo.Version);

            IsError = (LoadedMvcCoreAssemblies.Length != 1) || (LoadedMvcFuturesAssemblies.Length > 1) || IsFuturesConflict;
        }

        private static EnvironmentInformation GetEnvironmentInformation()
        {
            string iisVersion = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
            if (String.IsNullOrEmpty(iisVersion))
            {
                iisVersion = "Unknown";
            }

            string processName = "Unknown";
            try
            {
                // late binding so that LinkDemands are not triggered
                object currentProcess = typeof (Process).GetMethod("GetCurrentProcess", Type.EmptyTypes).Invoke(null,
                                                                                                                null);
                object processModule = typeof (Process).GetProperty("MainModule").GetValue(currentProcess, null);
                processName = (string) typeof (ProcessModule).GetProperty("ModuleName").GetValue(processModule, null);
            }
            catch
            {
            } // swallow exceptions

            return new EnvironmentInformation()
                       {
                           OperatingSystem = Environment.OSVersion,
                           NetFrameworkVersion = Environment.Version,
                           NetFrameworkBitness = IntPtr.Size*8,
                           ServerSoftware =
                               (iisVersion == "Unknown" && processName != null &&
                                processName.StartsWith("WebDev.WebServer", StringComparison.OrdinalIgnoreCase))
                                   ? "Visual Studio web server"
                                   : iisVersion,
                           WorkerProcess = processName,
                           IsIntegrated = HttpRuntime.UsingIntegratedPipeline
                       };
        }

        private static void PopulateLoadedAssemblyBaseInformation(LoadedAssemblyInfoBase assemblyInfo, Assembly assembly)
        {
            string codeBase = "Unknown";
            try
            {
                codeBase = assembly.CodeBase;
            }
            catch (SecurityException)
            {
                // can't read code base in medium trust, so just skip
            }

            string deployment = (assembly.GlobalAssemblyCache) ? "GAC" : "bin";

            assemblyInfo.CodeBase = codeBase;
            assemblyInfo.Deployment = deployment;
            assemblyInfo.FullName = assembly.FullName;
        }

        private static LoadedAssemblyInfo<MvcCoreAssemblyInfo> GetMvcAssemblyInformation(Assembly assembly)
        {
            AssemblyFileVersionAttribute fileVersionAttr =
                assembly.GetCustomAttributes(typeof (AssemblyFileVersionAttribute), true /* inherit */).OfType
                    <AssemblyFileVersionAttribute>().FirstOrDefault();

            string actualVersion = (fileVersionAttr != null) ? fileVersionAttr.Version : "no version";
            string friendlyName = "Unknown version";
            MvcCoreAssemblyInfo matchingCore =
                _mvcCoreAssemblyHistory.Where(c => String.Equals(actualVersion, c.Version)).FirstOrDefault();

            if (matchingCore == null)
            {
                matchingCore = new MvcCoreAssemblyInfo()
                                   {
                                       Name = friendlyName,
                                       Version = actualVersion
                                   };
            }

            LoadedAssemblyInfo<MvcCoreAssemblyInfo> assemblyInfo = new LoadedAssemblyInfo<MvcCoreAssemblyInfo>()
                                                                       {
                                                                           MvcAssemblyInfo = matchingCore
                                                                       };
            PopulateLoadedAssemblyBaseInformation(assemblyInfo, assembly);
            return assemblyInfo;
        }

        private static LoadedAssemblyInfo<MvcFuturesAssemblyInfo> GetFuturesAssemblyInformation(Assembly assembly)
        {
            AssemblyFileVersionAttribute fileVersionAttr =
                assembly.GetCustomAttributes(typeof (AssemblyFileVersionAttribute), true /* inherit */).OfType
                    <AssemblyFileVersionAttribute>().FirstOrDefault();

            string actualVersion = (fileVersionAttr != null) ? fileVersionAttr.Version : "no version";
            string friendlyName = "Unknown version";
            MvcFuturesAssemblyInfo matchingCore =
                _mvcFuturesAssemblyHistory.Where(c => String.Equals(actualVersion, c.Version)).FirstOrDefault();

            if (matchingCore == null)
            {
                matchingCore = new MvcFuturesAssemblyInfo()
                                   {
                                       Name = friendlyName,
                                       Version = actualVersion
                                   };
            }

            LoadedAssemblyInfo<MvcFuturesAssemblyInfo> assemblyInfo = new LoadedAssemblyInfo<MvcFuturesAssemblyInfo>()
                                                                          {
                                                                              MvcAssemblyInfo = matchingCore
                                                                          };
            PopulateLoadedAssemblyBaseInformation(assemblyInfo, assembly);
            return assemblyInfo;
        }

        private static bool IsMvcAssembly(Assembly assembly)
        {
            return (String.Equals(assembly.ManifestModule.Name, "System.Web.Mvc.dll", StringComparison.OrdinalIgnoreCase)
                    || (assembly.GetType("System.Web.Mvc.Controller", false /* throwOnError */) != null));
        }

        private static bool IsMvcFuturesAssembly(Assembly assembly)
        {
            return
                (String.Equals(assembly.ManifestModule.Name, "Microsoft.Web.Mvc.dll", StringComparison.OrdinalIgnoreCase));
        }

        public readonly EnvironmentInformation EnvironmentInformation;
        public readonly LoadedAssemblyInfo<MvcCoreAssemblyInfo>[] LoadedMvcCoreAssemblies;
        public readonly LoadedAssemblyInfo<MvcFuturesAssemblyInfo>[] LoadedMvcFuturesAssemblies;
        public readonly Assembly[] AllAssemblies;
        public readonly bool IsError;
        public readonly bool IsFuturesConflict;
    }

    public class EnvironmentInformation
    {
        public OperatingSystem OperatingSystem;
        public Version NetFrameworkVersion;
        public int NetFrameworkBitness;
        public string ServerSoftware;
        public string WorkerProcess;
        public bool IsIntegrated;
    }

    public class LoadedAssemblyInfo<T> : LoadedAssemblyInfoBase where T : MvcAssemblyInfoBase
    {
        public new T MvcAssemblyInfo
        {
            get { return (T) (base.MvcAssemblyInfo); }
            set { base.MvcAssemblyInfo = value; }
        }
    }

    public class LoadedAssemblyInfoBase
    {
        public MvcAssemblyInfoBase MvcAssemblyInfo;
        public string FullName;
        public string CodeBase;
        public string Deployment;
    }

    public class MvcCoreAssemblyInfo : MvcAssemblyInfoBase
    {
        public string FuturesVersion;
    }

    public class MvcFuturesAssemblyInfo : MvcAssemblyInfoBase
    {
        public string DownloadUrl;
    }

    public class MvcAssemblyInfoBase
    {
        public string Version;
        public string Name;
    }
}