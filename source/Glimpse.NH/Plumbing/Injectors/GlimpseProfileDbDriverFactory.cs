using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Glimpse.Ado.Plumbing;
using Glimpse.Core.Extensibility;
using Microsoft.CSharp;

namespace Glimpse.NH.Plumbing.Injectors
{
    public class GlimpseProfileDbDriverFactory : IGlimpseProfileDbDriverFactory
    {
        public Type GetProfileDbDriverType(Assembly nhibernateAssembly)
        {
            // Validate
            if (nhibernateAssembly == null)
                return null;

            // Determine the appropriate profiled driver version
            var version = nhibernateAssembly.GetName().Version;
            var versionNumber = string.Format("{0}{1}{2}", version.Major, version.Minor, version.Build);

            // Get the profiled driver code
            var code = GetEmbeddedResource(GetType().Assembly, string.Format("Glimpse.NH.Plumbing.Profiler.GlimpseProfileDbDriverNh{0}.cs", versionNumber));
            if (string.IsNullOrEmpty(code))
                return null;

            // Compile the profiled driver code
            var assembliesToReference = new[] { nhibernateAssembly, typeof(DbConnection).Assembly, typeof(TypeConverter).Assembly, typeof(IHttpModule).Assembly, typeof(IGlimpseFactory).Assembly, typeof(ProviderStats).Assembly, GetType().Assembly };
            var generatedAssembly = CreateAssembly(code, assembliesToReference);
            var generatedType = generatedAssembly.GetType(string.Format("Glimpse.NH.Plumbing.Profiler.GlimpseProfileDbDriverNh{0}", versionNumber));
            return generatedType;
        }

        private static string GetEmbeddedResource(Assembly assembly, string resourceName)
        {
            //See http://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static Assembly CreateAssembly(string code, IEnumerable<Assembly> referenceAssemblies)
        {
            //See http://stackoverflow.com/questions/3032391/csharpcodeprovider-doesnt-return-compiler-warnings-when-there-are-no-errors
            var provider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
            var compilerParameters = new CompilerParameters { GenerateExecutable = false, GenerateInMemory = true };
            compilerParameters.ReferencedAssemblies.AddRange(referenceAssemblies.Select(a => a.Location).ToArray());

            var results = provider.CompileAssemblyFromSource(compilerParameters, code);
            if (results.Errors.HasErrors)
                throw new InvalidOperationException(results.Errors[0].ErrorText);

            return results.CompiledAssembly;
        }
    }
}