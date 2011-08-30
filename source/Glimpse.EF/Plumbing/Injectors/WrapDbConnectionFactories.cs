using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Microsoft.CSharp;

namespace Glimpse.EF.Plumbing.Injectors
{
    public class WrapDbConnectionFactories : IWrapperInjectorProvider
    {
        private IGlimpseLogger Logger { get; set; }

        public WrapDbConnectionFactories(IGlimpseLogger logger)
        {
            Logger = logger;
        }

        public void Inject()
        {
            //TODO: Clean this up, what are we *really* testing here?
            var type = Type.GetType("System.Data.Entity.Database, EntityFramework", false);
            if (type != null && type.GetProperty("DefaultConnectionFactory") != null)
            {
                Logger.Info("AdoPipelineInitiator: Starting to inject ConnectionFactory");

                var code = GetEmbeddedResource("Glimpse.EF.Plumbing.Profiler.GlimpseProfileDbConnectionFactory.cs");
                var assembliesToReference = new[] { type.Assembly, typeof(DbConnection).Assembly, typeof(AdoPipelineInitiator).Assembly, typeof(TypeConverter).Assembly };

                var generatedAssembly = CreateAssembly(code, assembliesToReference);
                var generatedType = generatedAssembly.GetType("Glimpse.EF.Plumbing.Profiler.GlimpseProfileDbProviderFactory");
                generatedType.GetMethod("Initialize").Invoke(null, null);

                Logger.Info("AdoPipelineInitiator: Finished to inject ConnectionFactory");
            }

            Logger.Info("AdoPipelineInitiator: Finished trying to injecting DbConnectionFactory");
        }

        public static Assembly CreateAssembly(string code, IEnumerable<Assembly> referenceAssemblies)
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

        private static string GetEmbeddedResource(string resourceName)
        {
            //See http://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
