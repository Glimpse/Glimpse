using System.Data.Entity;
using System.Reflection; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework.Support;
using Glimpse.EF.AlternateType;

namespace Glimpse.EF.Inspector.Core
{
    public class DbConnectionFactoriesExecutionTask : IExecutionTask
    {
        public void Execute(ILogger logger)
        {
            logger.Info("EntityFrameworkInspector: Starting to replace DefaultConnectionFactory");

            var databaseType = typeof(Database);
            var defaultConnectionFactoryChanged = (bool)databaseType.GetProperty("DefaultConnectionFactoryChanged", BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic).GetValue(databaseType, null);

            if (defaultConnectionFactoryChanged)
            {
                logger.Info("EntityFrameworkInspector: Detected that user is using a custom DefaultConnectionFactory");

                Database.DefaultConnectionFactory = new GlimpseDbConnectionFactory(Database.DefaultConnectionFactory);
            }

            logger.Info("EntityFrameworkInspector: Finished to replacing DefaultConnectionFactory");
        }
    }
}