using System;
using System.Data;
using System.Data.Common;
using System.Linq; 
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework.Support;

namespace Glimpse.Ado.Inspector.Core
{
    public class DbProviderFactoriesExecutionTask : IExecutionTask
    {
        public void Execute(ILogger logger)
        { 
            logger.Info("AdoInspector: Starting to replace DbProviderFactory");

            //This forces the creation 
            try
            {
                DbProviderFactories.GetFactory("Anything"); 
            }
            catch (ArgumentException)
            { 
            }

            //Find the registered providers
            var table = Support.FindDbProviderFactoryTable();

            //Run through and replace providers
            foreach (var row in table.Rows.Cast<DataRow>().ToList())
            {
                DbProviderFactory factory;
                try
                {
                    factory = DbProviderFactories.GetFactory(row);

                    logger.Info("AdoInspector: Successfully retrieved factory - {0}", row["Name"]);
                }
                catch (Exception)
                {
                    logger.Error("AdoInspector: Failed to retrieve factory - {0}", row["Name"]);
                    continue;
                }

                //Check that we haven't already wrapped things up 
                if (factory is GlimpseDbProviderFactory)
                {
                    logger.Error("AdoInspector: Factory is already wrapped - {0}", row["Name"]);
                    continue;
                }

                var proxyType = typeof(GlimpseDbProviderFactory<>).MakeGenericType(factory.GetType());

                var newRow = table.NewRow();
                newRow["Name"] = row["Name"];
                newRow["Description"] = row["Description"];
                newRow["InvariantName"] = row["InvariantName"];
                newRow["AssemblyQualifiedName"] = proxyType.AssemblyQualifiedName;

                table.Rows.Remove(row);
                table.Rows.Add(newRow);

                logger.Info("AdoInspector: Successfully replaced - {0}", newRow["Name"]);
            }

            logger.Info("AdoInspector: Finished replacing DbProviderFactory");
        }
    }
}