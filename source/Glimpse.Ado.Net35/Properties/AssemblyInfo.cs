using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: ComVisible(false)]
[assembly: Guid("b5a203a0-e464-485c-abc5-4c7f26871dd4")]

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

[assembly: InternalsVisibleTo("Glimpse.Test.ADO")]
[assembly: NuGetPackage("Glimpse.Ado")]