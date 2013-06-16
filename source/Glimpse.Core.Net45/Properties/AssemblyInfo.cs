using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: ComVisible(false)]
[assembly: Guid("3b7a68a9-2d81-49c9-9838-c72698176b9c")]

[assembly: AssemblyTitle("Glimpse Core Assembly")]
[assembly: AssemblyDescription("Core interfaces and types for Glimpse.")]// When you right-click the assembly file in Windows Explorer, this attribute appears as the Comments value on the Version tab of the file properties dialog box.
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

// Version is in major.minor.build format to support http://semver.org/
// Keep these three attributes in sync
[assembly: AssemblyVersion("1.4.2")]
[assembly: AssemblyFileVersion("1.4.2")]
[assembly: AssemblyInformationalVersion("1.4.2")] // Used to specify the NuGet version number at build time

[assembly: InternalsVisibleTo("Glimpse.Test.Core")]
[assembly: NuGetPackage("Glimpse")]