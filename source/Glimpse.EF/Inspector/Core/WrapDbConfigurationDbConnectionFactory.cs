using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Config;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.EF.AlternateType;

namespace Glimpse.EF.Inspector.Core
{
    public class WrapDbConfigurationDbConnectionFactory
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
            newRow["AssemblyQualifiedName"] = typeof(GlimpseDbProviderFactory<System.Data.Entity.Core.EntityClient.EntityProviderFactory>).FullName;

            table.Rows.Add(newRow);


            ProviderRowFinder.Test();
        }
    }


    internal class ProviderRowFinder
    { 
        public static void Test()
        { 
            var dataRows = Support.FindDbProviderFactoryTable().Rows.OfType<DataRow>();
            var factory = typeof(GlimpseDbProviderFactory<System.Data.Entity.Core.EntityClient.EntityProviderFactory>);

            var row = FindRow(
                factory.GetType(),
                r => DbProviderFactories.GetFactory(r).GetType() == factory.GetType(),
                dataRows);

            var test = 1;
        }

        public static DataRow FindRow(Type hintType, Func<DataRow, bool> selector, IEnumerable<DataRow> dataRows)
        { 
            const int assemblyQualifiedNameIndex = 3;

            var assemblyHint = hintType == null ? null : new AssemblyName(hintType.Assembly.FullName);

            foreach (var row in dataRows)
            {
                var assemblyQualifiedTypeName = (string)row[assemblyQualifiedNameIndex];

                AssemblyName rowProviderFactoryAssemblyName = null;

                // Parse the provider factory assembly qualified type name
                Type.GetType(
                    assemblyQualifiedTypeName,
                    a =>
                    {
                        rowProviderFactoryAssemblyName = a;

                        return null;
                    },
                    (_, __, ___) => null);

                if (rowProviderFactoryAssemblyName != null
                    && (hintType == null
                        || string.Equals(
                            assemblyHint.Name,
                            rowProviderFactoryAssemblyName.Name,
                            StringComparison.OrdinalIgnoreCase)))
                {
                    try
                    {
                        if (selector(row))
                        {
                            return row;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Fail("GetFactory failed with: " + ex);
                        // Ignore bad providers.
                    }
                }
            }

            return null;
        }
    }
}
