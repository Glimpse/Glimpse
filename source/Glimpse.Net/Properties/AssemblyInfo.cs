using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using Glimpse.Net;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Glimpse Web Debugger (Beta)")]
[assembly: AssemblyDescription("Glimpse is a web debugger used to gain a better understanding of whats happening inside of your webserver. (Beta)")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("nmolnar, avanderhoorn")]
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4e613cbe-2441-4dc4-a812-4e6bba03e25c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.80.0.0")]
[assembly: AssemblyFileVersion("0.80.0.0")]

[assembly: System.Web.UI.WebResource("Glimpse.Net.glimpseClient.js", "application/x-javascript")]
[assembly: System.Web.UI.WebResource("Glimpse.Net.glimpseSprinte.png", "image/png")]

[assembly: PreApplicationStartMethod(typeof(PreApplicationStartCode), "Start")]
