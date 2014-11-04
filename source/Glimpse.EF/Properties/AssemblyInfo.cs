using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("61266d72-5987-460b-9536-eb164c9e0b4b")]

[assembly: AssemblyTitle("Glimpse EF Assembly")]
[assembly: AssemblyDescription("Glimpse extensions and tabs for EF.")]
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("© 2015 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0-alpha0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha0")] 

[assembly: InternalsVisibleTo("Glimpse.Test.EF")]

#if EF43 
    [assembly: NuGetPackage("Glimpse.EF43")]   
#elif EF5
    [assembly: NuGetPackage("Glimpse.EF5")]
#elif EF6
    [assembly: NuGetPackage("Glimpse.EF6")]   
#else
    [assembly: NuGetPackage("Glimpse.EF")]   
#endif