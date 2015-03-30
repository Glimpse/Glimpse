using System;
using System.Data.Common;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

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

        public override bool CanCreateDataSourceEnumerator
        {
            get { return InnerFactory.CanCreateDataSourceEnumerator; }
        }

        private TProviderFactory InnerFactory { get; set; }

        private static bool ShouldNotWrap()
        {
            if (GlimpseConfiguration.GetConfiguredMessageBroker() == null)
            {
                return true;
            }
            if (GlimpseConfiguration.GetRuntimePolicyStategy()() == RuntimePolicy.Off) 
            {
                return true;
            }
            return false;
        }

        public override DbCommand CreateCommand()
        {
            var command = InnerFactory.CreateCommand();
            if (ShouldNotWrap()) 
            {
                return command;
            }
            return new GlimpseDbCommand(command);
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return InnerFactory.CreateCommandBuilder();
        }

        public override DbConnection CreateConnection()
        {
            var connection = InnerFactory.CreateConnection();
            if (ShouldNotWrap())
            {
                return connection;
            }
            return new GlimpseDbConnection(connection, this);
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return InnerFactory.CreateConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            var adapter = InnerFactory.CreateDataAdapter();
            if (ShouldNotWrap())
            {
                return adapter;
            }
            return new GlimpseDbDataAdapter(adapter);
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
            {
                return InnerFactory;
            }

            var service = ((IServiceProvider)InnerFactory).GetService(serviceType);

            // HACK: To make things easier on ourselves we are going to try and see
            // what we can do for people using EF. If they are using EF but don't have
            // Glimpse.EF then we throw because the exception that will be caused down 
            // the track by EF isn't obvious as to whats going on. When it gets to 
            // requesting DbProviderServices, if we don't return the profiled version, 
            // when GetDbProviderManifestToken is called, it passes in a GlimpseDbConnection rather than the inner connection. This is a problem because the GetDbProviderManifestToken trys to cast the connection to its concreat type
            if (serviceType.FullName == "System.Data.Common.DbProviderServices")
            {
                var type = Type.GetType("Glimpse.EF.AlternateType.GlimpseDbProviderServices, Glimpse.EF43", false);
                if (type == null)
                {
                    type = Type.GetType("Glimpse.EF.AlternateType.GlimpseDbProviderServices, Glimpse.EF5", false);
                }

                if (type == null)
                {
                    type = Type.GetType("Glimpse.EF.AlternateType.GlimpseDbProviderServices, Glimpse.EF6", false);
                }

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