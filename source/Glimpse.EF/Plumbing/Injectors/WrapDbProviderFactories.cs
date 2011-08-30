using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.EF.Plumbing.Profiler;

namespace Glimpse.EF.Plumbing.Injectors
{
    public class WrapDbProviderFactories : IWrapperInjectorProvider
    {
        private IGlimpseLogger Logger { get; set; }

        public WrapDbProviderFactories(IGlimpseLogger logger)
        {
            Logger = logger;
        }

        public void Inject()
        { 
            //This forces the creation 
            try
            {
                DbProviderFactories.GetFactory("Anything");
            }
            catch (ArgumentException ex)
            {
                Logger.Info("AdoPipelineInitiator: Expected DbProviderFactories exception due too the way the API works.", ex);
            } 

            var providers = GetRegisteredProviders();
            var registrationKeys = GetProviderKeys(providers);
              
            foreach (var key in registrationKeys)
            {
                DbProviderFactory factory;
                try
                {
                    factory = DbProviderFactories.GetFactory(key);

                    Logger.Info("AdoPipelineInitiator: Triggered ");

                }
                catch (Exception)
                {
                    continue;
                }

                var proxyType = GetProxyTypeForProvider(factory.GetType());

                SwapAssemblyName(providers, key, proxyType);
            }

            Logger.Info("AdoPipelineInitiator: Finished injecting DbProviderFactory");
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

        private static Type GetProxyTypeForProvider(Type factoryType)
        {
            return typeof(GlimpseProfileDbProviderFactory<>).MakeGenericType(factoryType);
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
