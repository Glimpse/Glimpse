using System.Data.Entity;
using System.Reflection; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework.Support;
using Glimpse.EF.AlternateType;

namespace Glimpse.EF.Inspector.Core
{
    public class DbConnectionFactoriesExecutionTask : IExecutionTask
    {
        public DbConnectionFactoriesExecutionTask(ILogger logger)
        {
            Logger = logger;
        }

        private ILogger Logger { get; set; }

        public void Execute()
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
    }
}