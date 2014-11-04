using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("751c5691-28de-4b8d-ba4d-dde77a91efa9")]

[assembly: AssemblyTitle("Glimpse for ASP.NET MVC Assembly")]
[assembly: AssemblyDescription("Glimpse extensions and tabs for ASP.NET MVC.")]
[assembly: AssemblyProduct("Glimpse.Mvc5")]
[assembly: AssemblyCopyright("© 2015 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0-alpha0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha0")]

[assembly: InternalsVisibleTo("Glimpse.Test.Mvc")]
 
#if MVC3 
    [assembly: NuGetPackage("Glimpse.Mvc3")]   
#elif MVC4
    [assembly: NuGetPackage("Glimpse.Mvc4")]
#elif MVC5
    [assembly: NuGetPackage("Glimpse.Mvc5")]   
#else
    [assembly: NuGetPackage("Glimpse.Mvc")]
#endif