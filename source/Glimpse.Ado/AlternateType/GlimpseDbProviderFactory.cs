using System;
using System.Data.Common;
using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.AlternateType
{
    public abstract class GlimpseDbProviderFactory : DbProviderFactory
    {
    }

    public class GlimpseDbProviderFactory<TProviderFactory> : GlimpseDbProviderFactory, IServiceProvider
        where TProviderFactory : DbProviderFactory
    {        
        public static readonly GlimpseDbProviderFactory<TProviderFactory> Instance;
        private static IInspectorContext inspectorContext;
        private readonly TProviderFactory inner;
        
        // TODO: this is a hack, but found no way to inject or locate as yet:
        public static IInspectorContext InspectorContext
        {
            get
            {
                if(inspectorContext == null)
                {
                    throw new InvalidOperationException("The Pipeline Inspector was not set!");
                }
                return inspectorContext;
            }
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                inspectorContext = value;
            }
        }

        static GlimpseDbProviderFactory()
        {
            //Needs to be a singleton - http://ljusberg.se/blogs/smorakning/archive/2005/11/28/Custom-Data-Provider-_2800_continued_2900_.aspx
            Instance = new GlimpseDbProviderFactory<TProviderFactory>();
        }

        public GlimpseDbProviderFactory()
        {            
            var field = typeof(TProviderFactory).GetField("Instance", BindingFlags.Public | BindingFlags.Static);
            if (field == null)
                throw new NotSupportedException("Provider doesn't have Instance property.");
            inner = (TProviderFactory)field.GetValue(null);           
        } 

        public override bool CanCreateDataSourceEnumerator
        {
            get { return inner.CanCreateDataSourceEnumerator; }
        }

        public override DbCommand CreateCommand()
        { 
            return new GlimpseDbCommand(inner.CreateCommand(), InspectorContext);
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return inner.CreateCommandBuilder();
        }

        public override DbConnection CreateConnection()
        { 
            return new GlimpseDbConnection(inner.CreateConnection(), this, InspectorContext, Guid.NewGuid());
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return inner.CreateConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new GlimpseDbDataAdapter(inner.CreateDataAdapter()); 
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return inner.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return inner.CreateParameter();
        }
         
        public object GetService(Type serviceType)
        {
            if (serviceType == GetType()) 
                return this.inner; 

            var service = ((IServiceProvider)this.inner).GetService(serviceType);
            var inner = service as DbProviderServices;
            if (inner != null)
                return new GlimpseDbProviderServices(inner, InspectorContext); 
            return service;
        }
    }
}