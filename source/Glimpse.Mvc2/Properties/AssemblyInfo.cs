using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: ComVisible(false)]
[assembly: Guid("5a345850-1121-4ffe-9dd2-ad0de3a45a3c")]

[assembly: AssemblyTitle("Glimpse for ASP.NET MVC 2 Assembly")]
[assembly: AssemblyDescription("Glimpse extensions and tabs for ASP.NET MVC 2.")]
[assembly: AssemblyProduct("Glimpse.Mvc2")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse")]

// Version is in major.minor.build format to support http://semver.org/
// Keep these three attributes in sync
[assembly: AssemblyVersion("1.5.1")]
[assembly: AssemblyFileVersion("1.5.1")]
[assembly: AssemblyInformationalVersion("1.5.1")] // Used to specify the NuGet version number at build time

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.Mvc2")]
[assembly: NuGetPackage("Glimpse.Mvc2")]