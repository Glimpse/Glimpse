using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Config; 
using System.Reflection; 
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.EF.AlternateType;

namespace Glimpse.EF.Inspector.Core
{
    public class EntityFrameworkExectutionBlock
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
                        InitDbConnectionFactories();
                        InitDbConfiguration();
                        InitDbProviderFactory();
                        hasInitalized = true;
                    }
                }
            }
        }

        private void InitDbConnectionFactories()
        {
            Logger.Info("EntityFrameworkInspector: Starting to replace DefaultConnectionFactory");

            var databaseType = typeof(Database);
            var defaultConnectionFactoryChanged = (bool)databaseType.GetProperty("DefaultConnectionFactoryChanged", BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic).GetValue(databaseType, null);

            if (defaultConnectionFactoryChanged)
            {
                Logger.Info("EntityFrameworkInspector: Detected that user is using a custom DefaultConnectionFactory");

                Database.DefaultConnectionFactory = new GlimpseDbConnectionFactory(Database.DefaultConnectionFactory);
            }

            Logger.Info("EntityFrameworkInspector: Finished to replacing DefaultConnectionFactory");
        }

        private void InitDbConfiguration()
        {
            DbConfiguration.SetConfiguration(new GlimpseDbConfiguration());
        }

        private void InitDbProviderFactory()
        { 
            //Find the registered providers 
            var table = Support.FindDbProviderFactoryTable();

            //Add in a new row so it can be discovered
            var newRow = table.NewRow();
            newRow["Name"] = "Entity Provider Factory";
            newRow["Description"] = "";
            newRow["InvariantName"] = typeof(System.Data.Entity.Core.EntityClient.EntityProviderFactory).FullName;
            newRow["AssemblyQualifiedName"] = typeof(GlimpseDbProviderFactory<System.Data.Entity.Core.EntityClient.EntityProviderFactory>).AssemblyQualifiedName;

            table.Rows.Add(newRow);
        }
    }
}
