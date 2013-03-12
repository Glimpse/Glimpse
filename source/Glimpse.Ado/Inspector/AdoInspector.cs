using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility; 

namespace Glimpse.Ado.Inspector
{
    public class AdoInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            InitDbProviderFactory(context);
        }

        private void InitDbProviderFactory(IInspectorContext context)
        {
            context.Logger.Info("AdoInspector: Starting to replace DbProviderFactory");  

            //This forces the creation 
            try
            {
                DbProviderFactories.GetFactory("Anything");

                context.Logger.Info("AdoInspector: Successfully triggered the discovery of DbProviderFactories"); 
            }
            catch (ArgumentException ex)
            {
                context.Logger.Error("AdoInspector: Failed to triggered the discovery of DbProviderFactories", ex);
            }

            //Find the registered providers
            var dbProviderFactories = typeof(DbProviderFactories); 
            var providerField = dbProviderFactories.GetField("_configTable", BindingFlags.NonPublic | BindingFlags.Static) ?? dbProviderFactories.GetField("_providerTable", BindingFlags.NonPublic | BindingFlags.Static);
            if (providerField == null)
            {
                context.Logger.Error("AdoInspector: DbProviderFactories doesn't have the required properties"); 
                return;
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

                    context.Logger.Info("AdoInspector: Successfully retrieved factory - {0}", row["Name"]); 
                }
                catch (Exception)
                {
                    context.Logger.Error("AdoInspector: Failed to retrieve factory - {0}", row["Name"]);
                    continue;
                }

                //Check that we haven't already wrapped things up 
                if (factory is GlimpseDbProviderFactory)
                {
                    context.Logger.Error("AdoInspector: Factory is already wrapped - {0}", row["Name"]);
                    continue;
                }

                var proxyType = typeof(GlimpseDbProviderFactory<>).MakeGenericType(factory.GetType()); 

                // TODO: this is a hack, but found no way to inject or locate as yet:
                var inspector = proxyType.GetProperty("InspectorContext");
                inspector.SetValue(null, context, null);

                var newRow = table.NewRow();
                newRow["Name"] = row["Name"];
                newRow["Description"] = row["Description"];
                newRow["InvariantName"] = row["InvariantName"];
                newRow["AssemblyQualifiedName"] = proxyType.AssemblyQualifiedName;

                table.Rows.Remove(row);
                table.Rows.Add(newRow);

                context.Logger.Error("AdoInspector: Successfully replaced - {0}", newRow["Name"]);
            }

            context.Logger.Info("AdoInspector: Finished replacing DbProviderFactory");  
        }
    }
}