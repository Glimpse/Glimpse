using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: ComVisible(false)]
[assembly: Guid("da3e9a24-8809-48d0-807d-bce41a878883")]


[assembly: AssemblyTitle("Glimpse Core Assembly for .NET 3.5")]
[assembly: AssemblyDescription("Core .NET 3.5 interfaces and types for Glimpse.")]//When you right-click the assembly file in Windows Explorer, this attribute appears as the Comments value on the Version tab of the file properties dialog box.
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]


// Version is in major.minor.build format to support http://semver.org/
// Keep these three attributes in sync
[assembly: AssemblyVersion("1.8.5")]
[assembly: AssemblyFileVersion("1.8.5")]
[assembly: AssemblyInformationalVersion("1.8.5")]

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.Core")]
[assembly: NuGetPackage("Glimpse")]