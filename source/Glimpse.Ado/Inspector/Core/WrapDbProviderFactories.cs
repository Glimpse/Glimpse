using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Ado.Inspector.Core
{
    public class WrapDbProviderFactories
    {
        private static object hasInitalizedLock = new object();
        private static bool hasInitalized = false;

        private ILogger logger;
        private ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = GlimpseConfiguration.GetLogger() ?? new NullLogger(); 
                }

                return logger;
            }
        }

        public void Execute()
        {
            if (!hasInitalized)
            {
                lock (hasInitalizedLock)
                {
                    if (!hasInitalized)
                    {
                        InnerExecute();
                        hasInitalized = true;
                    }
                }
            }
        }

        private void InnerExecute()
        { 
            Logger.Info("AdoInspector: Starting to replace DbProviderFactory");

            //This forces the creation 
            try
            {
                DbProviderFactories.GetFactory("Anything"); 
            }
            catch (ArgumentException ex)
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

                    Logger.Info("AdoInspector: Successfully retrieved factory - {0}", row["Name"]);
                }
                catch (Exception)
                {
                    Logger.Error("AdoInspector: Failed to retrieve factory - {0}", row["Name"]);
                    continue;
                }

                //Check that we haven't already wrapped things up 
                if (factory is GlimpseDbProviderFactory)
                {
                    Logger.Error("AdoInspector: Factory is already wrapped - {0}", row["Name"]);
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

                Logger.Info("AdoInspector: Successfully replaced - {0}", newRow["Name"]);
            }

            Logger.Info("AdoInspector: Finished replacing DbProviderFactory");
        }
    }
}