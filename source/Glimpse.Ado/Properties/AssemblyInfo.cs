using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using Glimpse.Ado;
using Glimpse.Core.Extensibility;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("f2d6bd18-342c-4ae1-a63a-252265c5c16d")]

[assembly: AssemblyTitle("Glimpse Ado Assembly")]
[assembly: AssemblyDescription("Ado interfaces and types for Glimpse.")]
[assembly: AssemblyProduct("Glimpse")]
[assembly: AssemblyCopyright("© 2015 Nik Molnar & Anthony van der Hoorn")]
[assembly: AssemblyTrademark("Glimpse™")]

[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0-alpha0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha0")]

[assembly: InternalsVisibleTo("Glimpse.Test.Ado")]

[assembly: NuGetPackage("Glimpse.Ado")]
[assembly: PreApplicationStartMethod(typeof(Initialize), "Start")]