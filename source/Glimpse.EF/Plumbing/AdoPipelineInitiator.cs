using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Microsoft.CSharp;

namespace Glimpse.EF.Plumbing
{
    internal class AdoPipelineInitiator
    {
        private IGlimpseLogger Logger { get; set; }

        internal AdoPipelineInitiator(IGlimpseLogger logger)
        {
            Logger = logger;
            
            try
            {
                DbProviderFactories.GetFactory("Anything");
            }
            catch (ArgumentException ex)
            {
                Logger.Info("Expected DbProviderFactories exception due too the way the API works.", ex);
            } 

        }

        internal void AdoProviders()
        {
            InjectDbProviderFactories();
            InjectDbConnectionFactories();
        }

        private static void InjectDbConnectionFactories()
        {
            //TODO: Clean this up, what are we *really* testing here?
            var type = Type.GetType("System.Data.Entity.Database, EntityFramework", false);
            if (type != null && type.GetProperty("DefaultConnectionFactory") != null)
            {
                var code = GetEmbeddedResource("Glimpse.EF.Plumbing.GlimpseProfileDbConnectionFactory.cs");
                var assembliesToReference = new[] { type.Assembly, typeof(DbConnection).Assembly, typeof(AdoPipelineInitiator).Assembly, typeof(System.ComponentModel.TypeConverter).Assembly };

                var generatedAssembly = CreateAssembly(code, assembliesToReference);
                var generatedType = generatedAssembly.GetType("Glimpse.EF.Plumbing.GlimpseProfileDbProviderFactory");
                generatedType.GetMethod("Initialize").Invoke(null, null); 
            }
        }

        private static void InjectDbProviderFactories()
        {
            var providers = GetRegisteredProviders();
            var registrationKeys = GetProviderKeys(providers);
              
            foreach (var key in registrationKeys)
            {
                DbProviderFactory factory;
                try
                {
                    factory = DbProviderFactories.GetFactory(key);
                }
                catch (Exception)
                {
                    continue;
                }

                var proxyType = GetProxyTypeForProvider(factory.GetType());

                SwapAssemblyName(providers, key, proxyType);
            } 
        }

        public static Assembly CreateAssembly(string code, IEnumerable<Assembly> referenceAssemblies)
        {
            //See http://stackoverflow.com/questions/3032391/csharpcodeprovider-doesnt-return-compiler-warnings-when-there-are-no-errors
            var provider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
            
            var compilerParameters = new CompilerParameters { GenerateExecutable = false, GenerateInMemory = true};
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
            using (var reader = new StreamReader(stream)){
                return reader.ReadToEnd();
            }
        }

        private static Type GetProxyTypeForProvider(Type factoryType)
        {
            return typeof(GlimpseProfileDbProviderFactory<>).MakeGenericType(factoryType);
        }

        private static DataTable GetRegisteredProviders()
        {
            var dbProviderFactories = typeof(DbProviderFactories);

            var configTable = dbProviderFactories.GetField("_configTable", BindingFlags.NonPublic | BindingFlags.Static);
            var providerTable = dbProviderFactories.GetField("_providerTable", BindingFlags.NonPublic | BindingFlags.Static);
            var table = (configTable ?? providerTable);

            if (table == null)
                throw new Exception("Can not get registered providers.");

            var registrations = table.GetValue(null);

            if (registrations is DataSet)
                return ((DataSet)registrations).Tables["DbProviderFactories"];

            return (DataTable)registrations;
        }

        private static IEnumerable<string> GetProviderKeys(DataTable providers)
        {
            return (from DataRow row in providers.Rows select (string)row["InvariantName"]).ToList();
        }

        private static void SwapAssemblyName(DataTable fromProviders, string withKey, Type newType)
        {
            var oldRow = fromProviders.Rows.Cast<DataRow>().First(dt => (string)dt["InvariantName"] == withKey);

            var newRow = fromProviders.NewRow();
            newRow["Name"] = oldRow["Name"];
            newRow["Description"] = oldRow["Description"];
            newRow["InvariantName"] = oldRow["InvariantName"];
            newRow["AssemblyQualifiedName"] = newType.AssemblyQualifiedName;
            
            fromProviders.Rows.Remove(oldRow);
            fromProviders.Rows.Add(newRow);
        }
    }
}