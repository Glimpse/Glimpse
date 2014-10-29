using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.EF;

[assembly: ComVisible(false)]
[assembly: Guid("9A9DDFC7-B342-4E2E-99D6-2657AB8E3627")]

[assembly: AssemblyTitle("Glimpse for EF 5.0 Assembly")]
[assembly: AssemblyDescription("Main extensibility implementations for running Glimpse with EF 5.0.")]
[assembly: AssemblyProduct("Glimpse.EF5")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

// Version is in major.minor.build format to support http://semver.org/
// Keep these three attributes in sync
[assembly: AssemblyVersion("1.6.5")]
[assembly: AssemblyFileVersion("1.6.5")]
[assembly: AssemblyInformationalVersion("1.6.5")] // Used to specify the NuGet version number at build time

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.EF")]
[assembly: NuGetPackage("Glimpse.EF5")]
[assembly: PreApplicationStartMethod(typeof(Initialize), "Start")]