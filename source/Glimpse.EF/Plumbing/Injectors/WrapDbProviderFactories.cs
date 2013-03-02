using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Glimpse.Ado.Plumbing.Profiler;
using Glimpse.Core.Extensibility;

namespace Glimpse.EF.Plumbing.Injectors
{
    public class WrapDbProviderFactories
    {
        private static IInspectorContext InspectorContext;

        public WrapDbProviderFactories(IInspectorContext inspectorContext)
        {
            InspectorContext = inspectorContext;
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
           //     Logger.Info("AdoPipelineInitiator for EF: Expected DbProviderFactories exception due too the way the API works.", ex);
            } 

            var providers = GetRegisteredProviders();
            var registrationKeys = GetProviderKeys(providers);
              
            foreach (var key in registrationKeys)
            {
                DbProviderFactory factory;
                try
                {
                    factory = DbProviderFactories.GetFactory(key);

                    //Logger.Info("AdoPipelineInitiator for EF: Triggered");

                }
                catch (Exception)
                {
                    continue;
                }

                var proxyType = GetProxyTypeForProvider(factory.GetType());

                SwapAssemblyName(providers, key, proxyType);
            }

            //Logger.Info("AdoPipelineInitiator for EF: Finished injecting DbProviderFactory");
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
            var type = typeof(GlimpseProfileDbProviderFactory<>).MakeGenericType(factoryType);
            var inspector = type.GetProperty("InspectorContext");
            inspector.SetValue(null, InspectorContext, null);
            return type;
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
