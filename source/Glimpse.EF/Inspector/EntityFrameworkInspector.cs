using System.Data.Common;
using System.Data.Entity;
using System.Reflection;
using Glimpse.Core.Extensibility; 
using Glimpse.EF.AlternateType;

namespace Glimpse.EF.Inspector
{
    public class EntityFrameworkInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            InitDbConnectionFactories(context);           
        }

        private void InitDbConnectionFactories(IInspectorContext context)
        {
            context.Logger.Info("EntityFrameworkInspector: Starting to replace DefaultConnectionFactory");  

            var databaseType = typeof(Database);
            var defaultConnectionFactoryChanged = (bool)databaseType.GetProperty("DefaultConnectionFactoryChanged", BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic).GetValue(databaseType, null);

            if (defaultConnectionFactoryChanged)
            {
                context.Logger.Info("EntityFrameworkInspector: Detected that user is using a custom DefaultConnectionFactory");  

                Database.DefaultConnectionFactory = new GlimpseDbConnectionFactory(Database.DefaultConnectionFactory, context);
            }

            context.Logger.Info("EntityFrameworkInspector: Finished to replacing DefaultConnectionFactory");  
        }
    }
}