using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using Glimpse.Ado;
using Glimpse.Core.Extensibility;

[assembly: ComVisible(false)]
[assembly: Guid("396138aa-b068-4a92-ae95-8a21cfb6d2dd")]

[assembly: AssemblyTitle("Glimpse for ADO Assembly")]
[assembly: AssemblyDescription("Main extensibility implementations for running Glimpse with ADO.")]
[assembly: AssemblyProduct("Glimpse.ADO")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

// Version is in major.minor.build format to support http://semver.org/
// Keep these three attributes in sync
[assembly: AssemblyVersion("1.5.1")]
[assembly: AssemblyFileVersion("1.5.1")]
[assembly: AssemblyInformationalVersion("1.5.1")] // Used to specify the NuGet version number at build time

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.Ado")]
[assembly: NuGetPackage("Glimpse.Ado")]
[assembly: PreApplicationStartMethod(typeof(Initialize), "Start")]