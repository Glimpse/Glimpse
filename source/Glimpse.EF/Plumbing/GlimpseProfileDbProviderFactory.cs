using System;
using System.Data.Common;
using System.Reflection;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbProviderFactory<TProviderFactory> : DbProviderFactory, IServiceProvider
        where TProviderFactory : DbProviderFactory
    {
        public static readonly GlimpseProfileDbProviderFactory<TProviderFactory> Instance;
        public static readonly ProviderStats Stats;
        private readonly TProviderFactory _inner;

        static GlimpseProfileDbProviderFactory()
        {
            //Needs to be a singleton - http://ljusberg.se/blogs/smorakning/archive/2005/11/28/Custom-Data-Provider-_2800_continued_2900_.aspx
            Instance = new GlimpseProfileDbProviderFactory<TProviderFactory>();
            Stats = new ProviderStats();
        }

        public GlimpseProfileDbProviderFactory()
        {
            var field = typeof(TProviderFactory).GetField("Instance", BindingFlags.Public | BindingFlags.Static);
            if (field == null)
                throw new NotSupportedException("Provider doesn't have Instance property.");
            _inner = (TProviderFactory)field.GetValue(null); 
        } 

        public override bool CanCreateDataSourceEnumerator
        {
            get { return _inner.CanCreateDataSourceEnumerator; }
        }

        public override DbCommand CreateCommand()
        { 
            return new GlimpseProfileDbCommand(_inner.CreateCommand(), Stats);
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return _inner.CreateCommandBuilder();
        }

        public override DbConnection CreateConnection()
        { 
            return new GlimpseProfileDbConnection(_inner.CreateConnection(), this, Stats, Guid.NewGuid());
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return _inner.CreateConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new GlimpseProfileDbDataAdapter(_inner.CreateDataAdapter()); 
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return _inner.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return _inner.CreateParameter();
        }
         
        public object GetService(Type serviceType)
        {
            if (serviceType == GetType()) 
                return _inner; 

            var service = ((IServiceProvider)_inner).GetService(serviceType);
            var inner = service as DbProviderServices;
            if (inner != null)
                return new GlimpseProfileDbProviderServices(inner, Stats); 
            return service;
        }
    }
}