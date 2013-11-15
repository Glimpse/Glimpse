using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: ComVisible(false)]
[assembly: Guid("030a43d1-3fb3-47b2-ad4d-42a00493480f")]

[assembly: AssemblyTitle("Glimpse for ASP.NET WebForms Assembly")]
[assembly: AssemblyDescription("Glimpse extensions and tabs for ASP.NET WebForms.")]
[assembly: AssemblyProduct("Glimpse.WebForms")]
[assembly: AssemblyCopyright("© 2013 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

// Version is in major.minor.build format to support http://semver.org/
// Keep these three attributes in sync
[assembly: AssemblyVersion("1.0.2")]
[assembly: AssemblyFileVersion("1.0.2")]
[assembly: AssemblyInformationalVersion("1.0.2")] // Used to specify the NuGet version number at build time

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.WebForms")]
[assembly: NuGetPackage("Glimpse.WebForms")]