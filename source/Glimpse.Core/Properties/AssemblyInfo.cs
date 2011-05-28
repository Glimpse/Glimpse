using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using Glimpse.Core;

[assembly: AssemblyTitle("Glimpse Web Debugger (Beta)")]
[assembly: AssemblyDescription("Glimpse is a web debugger used to gain a better understanding of whats happening inside of your webserver. (Beta)")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("nmolnar, avanderhoorn")]
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: Guid("4e613cbe-2441-4dc4-a812-4e6bba03e25c")]

[assembly: AssemblyVersion("0.83")]
[assembly: AssemblyFileVersion("0.83")]

[assembly: PreApplicationStartMethod(typeof(PreApplicationStartCode), "Start")]
