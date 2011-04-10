<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.Security" %>
<%@ Import Namespace="System.Web.Compilation" %>

<script runat="server">
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

    // Diagnostics routines
    private class DiagnosticsResults {
        public DiagnosticsResults() {
            EnvironmentInformation = GetEnvironmentInformation();
            AllAssemblies = BuildManager.GetReferencedAssemblies().OfType<Assembly>().Concat(AppDomain.CurrentDomain.GetAssemblies()).Distinct().OrderBy(o => o.FullName).ToArray();
            LoadedMvcCoreAssemblies = AllAssemblies.Where(IsMvcAssembly).Select<Assembly, LoadedAssemblyInfo<MvcCoreAssemblyInfo>>(GetMvcAssemblyInformation).ToArray();
            LoadedMvcFuturesAssemblies = AllAssemblies.Where(IsMvcFuturesAssembly).Select<Assembly, LoadedAssemblyInfo<MvcFuturesAssemblyInfo>>(GetFuturesAssemblyInformation).ToArray();

            IsFuturesConflict = (LoadedMvcCoreAssemblies.Length == 1 && LoadedMvcFuturesAssemblies.Length == 1 && LoadedMvcCoreAssemblies[0].MvcAssemblyInfo.FuturesVersion != LoadedMvcFuturesAssemblies[0].MvcAssemblyInfo.Version);
            IsError = (LoadedMvcCoreAssemblies.Length != 1) || (LoadedMvcFuturesAssemblies.Length > 1) || IsFuturesConflict;
        }

        private static EnvironmentInformation GetEnvironmentInformation() {
            string iisVersion = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
            if (String.IsNullOrEmpty(iisVersion)) {
                iisVersion = "Unknown";
            }

            string processName = "Unknown";
            try {
                // late binding so that LinkDemands are not triggered
                object currentProcess = typeof(Process).GetMethod("GetCurrentProcess", Type.EmptyTypes).Invoke(null, null);
                object processModule = typeof(Process).GetProperty("MainModule").GetValue(currentProcess, null);
                processName = (string)typeof(ProcessModule).GetProperty("ModuleName").GetValue(processModule, null);
            }
            catch { } // swallow exceptions

            return new EnvironmentInformation() {
                OperatingSystem = Environment.OSVersion,
                NetFrameworkVersion = Environment.Version,
                NetFrameworkBitness = IntPtr.Size * 8,
                ServerSoftware = (iisVersion == "Unknown" && processName != null && processName.StartsWith("WebDev.WebServer", StringComparison.OrdinalIgnoreCase)) ? "Visual Studio web server" : iisVersion,
                WorkerProcess = processName,
                IsIntegrated = HttpRuntime.UsingIntegratedPipeline
            };
        }

        private static void PopulateLoadedAssemblyBaseInformation(LoadedAssemblyInfoBase assemblyInfo, Assembly assembly) {
            string codeBase = "Unknown";
            try {
                codeBase = assembly.CodeBase;
            }
            catch (SecurityException) {
                // can't read code base in medium trust, so just skip
            }

            string deployment = (assembly.GlobalAssemblyCache) ? "GAC" : "bin";

            assemblyInfo.CodeBase = codeBase;
            assemblyInfo.Deployment = deployment;
            assemblyInfo.FullName = assembly.FullName;
        }

        private static LoadedAssemblyInfo<MvcCoreAssemblyInfo> GetMvcAssemblyInformation(Assembly assembly) {
            AssemblyFileVersionAttribute fileVersionAttr = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true /* inherit */).OfType<AssemblyFileVersionAttribute>().FirstOrDefault();

            string actualVersion = (fileVersionAttr != null) ? fileVersionAttr.Version : "no version";
            string friendlyName = "Unknown version";
            MvcCoreAssemblyInfo matchingCore = _mvcCoreAssemblyHistory.Where(c => String.Equals(actualVersion, c.Version)).FirstOrDefault();

            if (matchingCore == null) {
                matchingCore = new MvcCoreAssemblyInfo() {
                    Name = friendlyName,
                    Version = actualVersion
                };
            }

            LoadedAssemblyInfo<MvcCoreAssemblyInfo> assemblyInfo = new LoadedAssemblyInfo<MvcCoreAssemblyInfo>() {
                MvcAssemblyInfo = matchingCore
            };
            PopulateLoadedAssemblyBaseInformation(assemblyInfo, assembly);
            return assemblyInfo;
        }

        private static LoadedAssemblyInfo<MvcFuturesAssemblyInfo> GetFuturesAssemblyInformation(Assembly assembly) {
            AssemblyFileVersionAttribute fileVersionAttr = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true /* inherit */).OfType<AssemblyFileVersionAttribute>().FirstOrDefault();

            string actualVersion = (fileVersionAttr != null) ? fileVersionAttr.Version : "no version";
            string friendlyName = "Unknown version";
            MvcFuturesAssemblyInfo matchingCore = _mvcFuturesAssemblyHistory.Where(c => String.Equals(actualVersion, c.Version)).FirstOrDefault();

            if (matchingCore == null) {
                matchingCore = new MvcFuturesAssemblyInfo() {
                    Name = friendlyName,
                    Version = actualVersion
                };
            }

            LoadedAssemblyInfo<MvcFuturesAssemblyInfo> assemblyInfo = new LoadedAssemblyInfo<MvcFuturesAssemblyInfo>() {
                MvcAssemblyInfo = matchingCore
            };
            PopulateLoadedAssemblyBaseInformation(assemblyInfo, assembly);
            return assemblyInfo;
        }

        private static bool IsMvcAssembly(Assembly assembly) {
            return (String.Equals(assembly.ManifestModule.Name, "System.Web.Mvc.dll", StringComparison.OrdinalIgnoreCase)
                || (assembly.GetType("System.Web.Mvc.Controller", false /* throwOnError */) != null));
        }

        private static bool IsMvcFuturesAssembly(Assembly assembly) {
            return (String.Equals(assembly.ManifestModule.Name, "Microsoft.Web.Mvc.dll", StringComparison.OrdinalIgnoreCase));
        }

        public readonly EnvironmentInformation EnvironmentInformation;
        public readonly LoadedAssemblyInfo<MvcCoreAssemblyInfo>[] LoadedMvcCoreAssemblies;
        public readonly LoadedAssemblyInfo<MvcFuturesAssemblyInfo>[] LoadedMvcFuturesAssemblies;
        public readonly Assembly[] AllAssemblies;
        public readonly bool IsError;
        public readonly bool IsFuturesConflict;
    }

    private class EnvironmentInformation {
        public OperatingSystem OperatingSystem;
        public Version NetFrameworkVersion;
        public int NetFrameworkBitness;
        public string ServerSoftware;
        public string WorkerProcess;
        public bool IsIntegrated;
    }

    private class LoadedAssemblyInfo<T> : LoadedAssemblyInfoBase where T : MvcAssemblyInfoBase {
        public new T MvcAssemblyInfo {
            get {
                return (T)(base.MvcAssemblyInfo);
            }
            set {
                base.MvcAssemblyInfo = value;
            }
        }
    }

    private class LoadedAssemblyInfoBase {
        public MvcAssemblyInfoBase MvcAssemblyInfo;
        public string FullName;
        public string CodeBase;
        public string Deployment;
    }

    private class MvcCoreAssemblyInfo : MvcAssemblyInfoBase {
        public string FuturesVersion;
    }

    private class MvcFuturesAssemblyInfo : MvcAssemblyInfoBase {
        public string DownloadUrl;
    }

    private class MvcAssemblyInfoBase {
        public string Version;
        public string Name;
    }

    private static MvcFuturesAssemblyInfo GetOrCreateFuturesAssemblyInfo(string futuresVersion) {
        MvcFuturesAssemblyInfo futuresInfo = _mvcFuturesAssemblyHistory.FirstOrDefault(o => String.Equals(futuresVersion, o.Version, StringComparison.OrdinalIgnoreCase));
        return (futuresInfo != null) ? futuresInfo : new MvcFuturesAssemblyInfo() { Name = "ASP.NET MVC Futures", Version = futuresVersion };
    }

    private static string AE(object input) {
        return HttpUtility.HtmlAttributeEncode(Convert.ToString(input, CultureInfo.InvariantCulture));
    }

    private static string E(object input) {
        return HttpUtility.HtmlEncode(Convert.ToString(input, CultureInfo.InvariantCulture));
    }

    private static string IsAppDomainHomogenous(AppDomain appDomain) {
        // AppDomain.IsHomogenous didn't exist prior to .NET 4, so use Reflection to look it up
        PropertyInfo pInfo = typeof(AppDomain).GetProperty("IsHomogenous");
        if (pInfo == null) {
            return "unknown";
        }

        // MethodInfo.Invoke demands ReflectionPermission when the target is AppDomain, but since target method is transparent we can instantiate a Delegate instead
        return Convert.ToString(((Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), appDomain, pInfo.GetGetMethod()))());
    }

    private static string IsAssemblyFullTrust(Assembly assembly) {
        // Assembly.IsFullyTrusted didn't exist prior to .NET 4, so use Reflection to look it up
        PropertyInfo pInfo = typeof(Assembly).GetProperty("IsFullyTrusted");
        if (pInfo == null) {
            return "unknown";
        }

        // MethodInfo.Invoke demands ReflectionPermission when the target is Assembly, but since target method is transparent we can instantiate a Delegate instead
        return Convert.ToString(((Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), assembly, pInfo.GetGetMethod()))());
    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ASP.NET MVC Diagnostics Utility</title>
    <style type="text/css">
        .error 
        {
            font-weight: bold;
            color: Red;
        }
        
        .box 
        {
            border-width: thin;
            border-style: solid;
            padding: .2em 1em .2em 1em;
            background-color: #dddddd;
        }
        
        .errorInset
        {
            padding: 1em;
            background-color: #ffbbbb;
        }
        
        body 
        {
            font-family: Calibri, Helvetica;
            padding: 0;
            margin: 1em;
        }
    </style>
</head>

<%
    DiagnosticsResults results = new DiagnosticsResults();

    Action<LoadedAssemblyInfoBase> outputAssemblyInfo = assemblyInfo => {
        %>
        <p>
            <b>Assembly version:</b> <%= E(assemblyInfo.MvcAssemblyInfo.Name) %> (<%= E(assemblyInfo.MvcAssemblyInfo.Version) %>)<br />
            <b>Full name:</b> <%= E(assemblyInfo.FullName) %><br />
            <b>Code base:</b> <%= E(assemblyInfo.CodeBase) %><br />
            <b>Deployment:</b> <%= E(assemblyInfo.Deployment) %>-deployed
        </p>
    <% }; %>

<body>
    <h1>Microsoft ASP.NET MVC Diagnostics Information</h1>
    <p>
        This page is designed to help diagnose common errors related to mismatched or conflicting ASP.NET MVC binaries.
        If a known issue is identified, it will be displayed below in <span class="error">red</span> text.
    </p>
    <p>
        For questions or problems with ASP.NET MVC or this utility, please visit the ASP.NET MVC forums at <a href="http://forums.asp.net/1146.aspx">http://forums.asp.net/1146.aspx</a>.
    </p>
    <% if (results.IsError) { %><p class="error">Errors were found.  Please see below for more information.</p><% } %>
    <h2>Environment Information</h2>
    <div class="box">
        <p>
            <b>Operating system:</b> <%= E(results.EnvironmentInformation.OperatingSystem) %><br />
            <b>.NET Framework version:</b> <%= E(results.EnvironmentInformation.NetFrameworkVersion) %> (<%= E(results.EnvironmentInformation.NetFrameworkBitness) %>-bit)<br />
            <b>Web server:</b> <%= E(results.EnvironmentInformation.ServerSoftware) %><br />
            <b>Integrated pipeline:</b> <%= E(results.EnvironmentInformation.IsIntegrated) %><br />
            <b>Worker process:</b> <%= E(results.EnvironmentInformation.WorkerProcess) %><br />
            <b>AppDomain:</b> Homogenous = <%= E(IsAppDomainHomogenous(AppDomain.CurrentDomain)) %>, FullTrust = <%= E(IsAssemblyFullTrust(GetType().Assembly)) %>
        </p>
    </div>
    <h2>ASP.NET MVC Assembly Information (System.Web.Mvc.dll)</h2>
    <div class="box">
        <% if (results.LoadedMvcCoreAssemblies.Length == 0) { %><p class="error">An ASP.NET MVC assembly has not been loaded into this application.</p><% } %>
        <% if (results.LoadedMvcCoreAssemblies.Length > 1) { %><p class="error">Multiple ASP.NET MVC assemblies have been loaded into this application.</p><% } %>
        <% foreach (var info in results.LoadedMvcCoreAssemblies) { outputAssemblyInfo(info); } %>
    </div>
    <h2>ASP.NET MVC Futures Assembly Information (Microsoft.Web.Mvc.dll)</h2>
    <div class="box">
        <% if (results.LoadedMvcFuturesAssemblies.Length == 0) { %>
            <p>
                An ASP.NET MVC Futures assembly has not been loaded into this application.
                <% if (results.LoadedMvcCoreAssemblies.Length == 1) { %>
                    <% MvcFuturesAssemblyInfo futuresAssemblyInfo = GetOrCreateFuturesAssemblyInfo(results.LoadedMvcCoreAssemblies[0].MvcAssemblyInfo.FuturesVersion); %>
                    <% if (futuresAssemblyInfo.DownloadUrl == null) { %>
                        A download for this version of ASP.NET MVC Futures is not available. Pre-release versions of the MVC Futures assembly are no longer available once that version of ASP.NET MVC has gone RTM.
                    <% } else { %>
                        If desired, you can download <%= E(futuresAssemblyInfo.Name) %> from <a href="<%= AE(futuresAssemblyInfo.DownloadUrl) %>"><%= E(futuresAssemblyInfo.DownloadUrl) %></a>.
                    <% } %>
                <% } %>
            </p>
        <% } %>
        <% if (results.LoadedMvcFuturesAssemblies.Length > 1) { %><p class="error">Multiple ASP.NET MVC Futures assemblies have been loaded for this application.</p><% } %>
        <% if (results.IsFuturesConflict) { %>
            <%-- We know that there's one version of Futures + one version of MVC loaded. --%>
            <div>
                <p class="error">Mismatched or outdated versions of ASP.NET MVC and ASP.NET MVC Futures are loaded.</p>
                <p class="errorInset">
                    Loaded version of ASP.NET MVC is: <%= E(results.LoadedMvcCoreAssemblies[0].MvcAssemblyInfo.Name)%> (<%= E(results.LoadedMvcCoreAssemblies[0].MvcAssemblyInfo.Version) %>)<br />
                    Loaded version of ASP.NET MVC Futures is: <%= E(results.LoadedMvcFuturesAssemblies[0].MvcAssemblyInfo.Name) %> (<%= E(results.LoadedMvcFuturesAssemblies[0].MvcAssemblyInfo.Version) %>)<br />
                    <% MvcFuturesAssemblyInfo futuresAssemblyInfo = GetOrCreateFuturesAssemblyInfo(results.LoadedMvcCoreAssemblies[0].MvcAssemblyInfo.FuturesVersion); %>
                    <% if (futuresAssemblyInfo.DownloadUrl == null) { %>
                        A download for this version of ASP.NET MVC Futures is not available. Pre-release versions of the MVC Futures assembly are no longer available once that version of ASP.NET MVC has gone RTM.
                    <% } else { %>
                        Download <%= E(futuresAssemblyInfo.Name) %> from <a href="<%= AE(futuresAssemblyInfo.DownloadUrl) %>"><%= E(futuresAssemblyInfo.DownloadUrl) %></a>.
                    <% } %>
                </p>
            </div>
        <% } %>
        <% foreach (var info in results.LoadedMvcFuturesAssemblies) { outputAssemblyInfo(info); } %>
    </div>
    <h2>All Loaded Assemblies</h2>
    <div class="box">
        <p><%= E(results.AllAssemblies.Length) %> assemblies are loaded.</p>
        <ul>
            <% foreach (Assembly assembly in results.AllAssemblies) { %><li><%= E(assembly) %></li><% } %>
        </ul>
    </div>
    <p>
        <b>Diagnostics version:</b> <%= E(_utilityDate.ToString("D")) %> (<%= E(_utilityVersion) %>)<br />
        <b>Report generated on:</b> <%= E(DateTime.Now.ToString("F")) %>
    </p>
</body>
</html>
