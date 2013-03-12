using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.Inspector.Support
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
                //Logger.Info("AdoPipelineInitiator: Expected DbProviderFactories exception due too the way the API works.", ex);
            }

            //Find the registered providers
            var dbProviderFactories = typeof(DbProviderFactories); 

            var providerField = dbProviderFactories.GetField("_configTable", BindingFlags.NonPublic | BindingFlags.Static) ?? dbProviderFactories.GetField("_providerTable", BindingFlags.NonPublic | BindingFlags.Static);
            if (providerField == null)
            {
                throw new Exception("Can not get registered providers.");
            }

            var registrations = providerField.GetValue(null);

            var table = registrations is DataSet ? ((DataSet)registrations).Tables["DbProviderFactories"] : (DataTable)registrations;  

            //Run through and replace providers
            foreach (var row in table.Rows.Cast<DataRow>().ToList())
            {
                DbProviderFactory factory;
                try
                {
                    factory = DbProviderFactories.GetFactory(row);

                    //Logger.Info("AdoPipelineInitiator: Triggered"); 
                }
                catch (Exception)
                {
                    continue;
                }
                 
                var proxyType = typeof(GlimpseDbProviderFactory<>).MakeGenericType(factory.GetType());

                // TODO: this is a hack, but found no way to inject or locate as yet:
                var inspector = proxyType.GetProperty("InspectorContext");
                inspector.SetValue(null, InspectorContext, null);

                var newRow = table.NewRow();
                newRow["Name"] = row["Name"];
                newRow["Description"] = row["Description"];
                newRow["InvariantName"] = row["InvariantName"];
                newRow["AssemblyQualifiedName"] = proxyType.AssemblyQualifiedName;

                table.Rows.Remove(row);
                table.Rows.Add(newRow);
            }

            //Logger.Info("AdoPipelineInitiator: Finished injecting DbProviderFactory");
        } 
    }
}
