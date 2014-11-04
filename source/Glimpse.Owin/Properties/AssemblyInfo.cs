using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("230a43d1-3fb3-47b2-ad4d-42a00493480f")]

[assembly: AssemblyTitle("Glimpse for Owin Assembly")]
[assembly: AssemblyDescription("Glimpse extensions and tabs for Owin.")]
[assembly: AssemblyProduct("Glimpse.Owin")]
[assembly: AssemblyCopyright("© 2015 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0-alpha0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha0")]

[assembly: InternalsVisibleTo("Glimpse.Test.Owin")]

[assembly: NuGetPackage("Glimpse.Owin")]