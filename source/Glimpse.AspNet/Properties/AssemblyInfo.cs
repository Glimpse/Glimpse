using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("251c5691-28de-4b8d-ba4d-dde77a91efa9")]

[assembly: AssemblyTitle("Glimpse for ASP.NET Assembly")]
[assembly: AssemblyDescription("Glimpse extensions and tabs for ASP.NET.")]
[assembly: AssemblyProduct("Glimpse.AspNet")]
[assembly: AssemblyCopyright("© 2015 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0-alpha0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha0")] // Used to specify the NuGet version number at build time

[assembly: InternalsVisibleTo("Glimpse.Test.AspNet")]

[assembly: NuGetPackage("Glimpse.AspNet")]