using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("030a43d1-3fb3-47b2-ad4d-42a00493480f")]

[assembly: AssemblyTitle("Glimpse for ASP.NET WebForms Assembly")]
[assembly: AssemblyDescription("Glimpse extensions and tabs for ASP.NET WebForms.")]
[assembly: AssemblyProduct("Glimpse.WebForms")]
[assembly: AssemblyCopyright("© 2015 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0-alpha0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha0")] 

[assembly: InternalsVisibleTo("Glimpse.Test.WebForms")]

[assembly: NuGetPackage("Glimpse.WebForms")]