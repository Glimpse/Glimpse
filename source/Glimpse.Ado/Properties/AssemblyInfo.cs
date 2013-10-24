using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: AssemblyTitle("Glimpse Ado Assembly")]
[assembly: AssemblyDescription("Ado interfaces and types for Glimpse.")]// When you right-click the assembly file in Windows Explorer, this attribute appears as the Comments value on the Version tab of the file properties dialog box.
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f2d6bd18-342c-4ae1-a63a-252265c5c16d")]

[assembly: AssemblyVersion("1.6.0")]
[assembly: AssemblyFileVersion("1.6.0")]
[assembly: AssemblyInformationalVersion("1.6.0-RC1")] // Used to specify the NuGet version number at build time

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.Ado")]
[assembly: NuGetPackage("Glimpse.Ado")]