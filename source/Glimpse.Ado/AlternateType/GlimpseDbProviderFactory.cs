using System;
using System.Data.Common;
using System.Reflection; 

namespace Glimpse.Ado.AlternateType
{
    public abstract class GlimpseDbProviderFactory : DbProviderFactory
    {
    }

    public class GlimpseDbProviderFactory<TProviderFactory> : GlimpseDbProviderFactory, IServiceProvider
        where TProviderFactory : DbProviderFactory
    {   
        public static readonly GlimpseDbProviderFactory<TProviderFactory> Instance = new GlimpseDbProviderFactory<TProviderFactory>();
        
        public GlimpseDbProviderFactory()
        {            
            var field = typeof(TProviderFactory).GetField("Instance", BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                throw new NotSupportedException("Provider doesn't have Instance property.");
            }

            InnerFactory = (TProviderFactory)field.GetValue(null);           
        }

        private TProviderFactory InnerFactory { get; set; }

        public override bool CanCreateDataSourceEnumerator
        {
            get { return InnerFactory.CanCreateDataSourceEnumerator; }
        }

        public override DbCommand CreateCommand()
        { 
            return new GlimpseDbCommand(InnerFactory.CreateCommand());
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return InnerFactory.CreateCommandBuilder();
        }

        public override DbConnection CreateConnection()
        { 
            return new GlimpseDbConnection(InnerFactory.CreateConnection(), this);
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return InnerFactory.CreateConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new GlimpseDbDataAdapter(InnerFactory.CreateDataAdapter()); 
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return InnerFactory.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return InnerFactory.CreateParameter();
        }
         
        public object GetService(Type serviceType)
        {
            if (serviceType == GetType()) 
                return InnerFactory;

            var service = ((IServiceProvider)InnerFactory).GetService(serviceType);

            // HACK: To make things easier on ourselves we are going to try and see
            // what we can do for people using EF. If they are using EF but don't have
            // Glimpse.EF then we throw because the exception that will be caused down 
            // the track by EF isn't obvious as to whats going on. When it gets to 
            // requesting DbProviderServices, if we don't return the profiled version, 
            // when GetDbProviderManifestToken is called, it passes in a GlimpseDbConnection rather than the inner connection. This is a problem because the GetDbProviderManifestToken trys to cast the connection to its concreat type
            if (serviceType.FullName == "System.Data.Common.DbProviderServices")
            {
                var type = Type.GetType("Glimpse.EF.AlternateType.GlimpseDbProviderServices, Glimpse.EF", false);
                if (type != null)
                {
                    return Activator.CreateInstance(type, service);
                } 
                
                throw new NotSupportedException(Resources.GlimpseEFNotPresent);  
            }

            return service;
        }
    }
}