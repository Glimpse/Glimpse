using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Glimpse.Core.Extensibility;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("3b7a68a9-2d81-49c9-9838-c72698176b9c")]

[assembly: AssemblyTitle("Glimpse Core Assembly")]
[assembly: AssemblyDescription("Core interfaces and types for Glimpse.")]
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("© 2015 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0-alpha0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha0")] 

[assembly: InternalsVisibleTo("Glimpse.Test.Core")]
[assembly: InternalsVisibleTo("Glimpse.Test.AspNet")]

[assembly: NuGetPackage("Glimpse")]