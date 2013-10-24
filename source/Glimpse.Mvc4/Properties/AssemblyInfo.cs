using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: ComVisible(false)]
[assembly: Guid("751c5691-28de-4b8d-ba4d-dde77a91efa9")]

[assembly: AssemblyTitle("Glimpse for ASP.NET MVC 4 Assembly")]
[assembly: AssemblyDescription("Glimpse extensions and tabs for ASP.NET MVC 4.")]
[assembly: AssemblyProduct("Glimpse.Mvc4")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

// Version is in major.minor.build format to support http://semver.org/
// Keep these three attributes in sync
[assembly: AssemblyVersion("1.3.3")]
[assembly: AssemblyFileVersion("1.3.3")]
[assembly: AssemblyInformationalVersion("1.3.3")] // Used to specify the NuGet version number at build time

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.Mvc4")]
[assembly: NuGetPackage("Glimpse.Mvc4")]