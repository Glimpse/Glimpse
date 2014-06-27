using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: AssemblyTitle("Glimpse EF Assembly")]
[assembly: AssemblyDescription("EF interfaces and types for Glimpse.")]// When you right-click the assembly file in Windows Explorer, this attribute appears as the Comments value on the Version tab of the file properties dialog box.
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("© 2012 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("61266d72-5987-460b-9536-eb164c9e0b4b")]

[assembly: AssemblyVersion("1.6.3")]
[assembly: AssemblyFileVersion("1.6.3")]
[assembly: AssemblyInformationalVersion("1.6.3")] // Used to specify the NuGet version number at build time

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Glimpse.Test.EF")]
[assembly: NuGetPackage("Glimpse.EF")]