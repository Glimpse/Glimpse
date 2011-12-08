using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using Glimpse.Ado.Plumbing;
using Glimpse.Core.Extensibility;
using Microsoft.CSharp;

namespace Glimpse.NH.Plumbing.Injectors
{
    public class WrapDbDriver
    {
        private IGlimpseLogger Logger { get; set; }

        public WrapDbDriver(IGlimpseLogger logger)
        {
            Logger = logger;
        }

        public void Inject()
        {
            var sessionFactoryObjectFactoryType = Type.GetType("NHibernate.Impl.SessionFactoryObjectFactory, NHibernate", false, true);
            if (sessionFactoryObjectFactoryType == null)
                return;
            
            var intancesField = sessionFactoryObjectFactoryType.GetField("Instances", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static);
            if (intancesField == null)
                return;

            var sessionFactoryImplType = Type.GetType("NHibernate.Impl.SessionFactoryImpl, NHibernate", false, true);
            if (sessionFactoryImplType == null)
                return;

            var settingsField = sessionFactoryImplType.GetField("Settings", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance);
            if (settingsField == null)
                return;

            var settingsType = Type.GetType("NHibernate.Cfg.Settings, NHibernate", false, true);
            if (settingsType == null)
                return;
            
            var connectionProviderField = settingsType.GetProperty("ConnectionProvider", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            if (connectionProviderField == null)
                return;

            var connectionProviderType = Type.GetType("NHibernate.Connection.ConnectionProvider, NHibernate", false, true);
            if (connectionProviderType == null)
                return;

            var driverField = connectionProviderType.GetField("Driver", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance);
            if (driverField == null)
                return;

            var intances = (IDictionary)intancesField.GetValue(null);
            foreach (DictionaryEntry intance in intances)
            {
                // Get the driver to wrap
                var settings = settingsField.GetValue(intance.Value);
                var connectionProvider = connectionProviderField.GetValue(settings, BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance, null, null, null);
                var driver = driverField.GetValue(connectionProvider);
                var driverVersion = sessionFactoryObjectFactoryType.Assembly.GetName().Version;
                var driverVersionNumber = string.Format("{0}{1}{2}", driverVersion.Major, driverVersion.Minor, driverVersion.Build);

                // Compile the profiled driver code
                var profileDriverTypeCode = GetEmbeddedResource(GetType().Assembly, string.Format("Glimpse.NH.Plumbing.Profiler.GlimpseProfileDbDriverNh{0}.cs", driverVersionNumber));
                var profileDriverTypeAssembliesToReference = new[] { driver.GetType().Assembly, typeof(DbConnection).Assembly, typeof(TypeConverter).Assembly, typeof(ProviderStats).Assembly };
                var profileDriverTypeGeneratedAssembly = CreateAssembly(profileDriverTypeCode, profileDriverTypeAssembliesToReference);
                var profileDriverTypeGeneratedType = profileDriverTypeGeneratedAssembly.GetType(string.Format("Glimpse.NH.Plumbing.Profiler.GlimpseProfileDbDriverNh{0}`1", driverVersionNumber));

                // Wrap the driver into the profiled driver
                var profiledDriverType = profileDriverTypeGeneratedType.MakeGenericType(driver.GetType());
                var profiledDriverTypeConstructor = profiledDriverType.GetConstructor(new[] {driver.GetType()});
                var profiledDriver = profiledDriverTypeConstructor.Invoke(new[] {driver});
                driverField.SetValue(connectionProvider, profiledDriver, BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance, null, null);
            }

            Logger.Info("AdoPipelineInitiator for NH: Finished wrapping the NHibernate DbDriver");
        }

        private static string GetEmbeddedResource(Assembly assembly, string resourceName)
        {
            //See http://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
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