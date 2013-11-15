using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.EF;

[assembly: ComVisible(false)]
[assembly: Guid("1C1FABFD-768A-4E03-88DD-14E04F6B4DD8")]

[assembly: AssemblyTitle("Glimpse for EF 6.0 Assembly")]
[assembly: AssemblyDescription("Main extensibility implementations for running Glimpse with EF 6.0.")]
[assembly: AssemblyProduct("Glimpse.EF6")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

// Version is in major.minor.build format to support http://semver.org/
// Keep these three attributes in sync
[assembly: AssemblyVersion("1.6.0")]
[assembly: AssemblyFileVersion("1.6.0")]
[assembly: AssemblyInformationalVersion("1.6.0")] // Used to specify the NuGet version number at build time

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.EF")]
[assembly: NuGetPackage("Glimpse.EF6")]
[assembly: PreApplicationStartMethod(typeof(Initialize), "Start")]