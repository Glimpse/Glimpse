using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbConnection : DbConnection
    {
        private DbConnection Inner { get; set; }
        private readonly DbProviderFactory _providerFactory;
        private readonly ProviderStats _stats;
        private readonly Guid _connectionId;


        public GlimpseProfileDbConnection(DbConnection inner, DbProviderFactory providerFactory, ProviderStats stats, Guid connectionId)
        {
            Inner = inner;
            _providerFactory = providerFactory;
            _stats = stats;
            _connectionId = connectionId;
            
            _stats.ConnectionStarted(_connectionId);
        }

          
        public override string ConnectionString
        {
            get { return Inner.ConnectionString; }
            set { Inner.ConnectionString = value; }
        }

        public override int ConnectionTimeout
        {
            get { return Inner.ConnectionTimeout; }
        }

        public override string Database
        {
            get { return Inner.Database; }
        }

        public override string DataSource
        {
            get { return Inner.DataSource; }
        }

        protected override DbProviderFactory DbProviderFactory
        {
            get { return _providerFactory; }
        }

        public override ConnectionState State
        {
            get { return Inner.State; }
        }

        public override string ServerVersion
        {
            get { return Inner.ServerVersion; }
        }

        public override ISite Site
        {
            get { return Inner.Site; }
            set { Inner.Site = value; }
        }


        public event StateChangeEventHandler StateChange
        {
            add { Inner.StateChange += value; }
            remove { Inner.StateChange -= value; }
        }


        protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {
            //return this._inner.BeginTransaction(isolationLevel);
            return new GlimpseProfileDbTransaction(Inner.BeginTransaction(isolationLevel), _stats, this);
        }

        public override void ChangeDatabase(string databaseName)
        {
            Inner.ChangeDatabase(databaseName);
        }

        protected override DbCommand CreateDbCommand()
        {
            //return this._inner.CreateCommand();
            return new GlimpseProfileDbCommand(Inner.CreateCommand(), _stats);
        }

        public override void Close()
        {
            Inner.Close();
            NotifyClosing();
        }

        public override void Open()
        {
            Inner.Open();
        }

        public override void EnlistTransaction(Transaction transaction)
        {
            Inner.EnlistTransaction(transaction);
            if (transaction != null)
            {
                transaction.TransactionCompleted += OnDtcTransactionCompleted;
                _stats.DtcTransactionEnlisted(_connectionId, transaction.IsolationLevel);
            }
        }
         
        public override DataTable GetSchema()
        {
            return Inner.GetSchema();
        }

        public override DataTable GetSchema(string collectionName)
        {
            return Inner.GetSchema(collectionName);
        }

        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            return Inner.GetSchema(collectionName, restrictionValues);
        }

        protected override object GetService(Type service)
        {
            return ((IServiceProvider)Inner).GetService(service);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                //this.NotifyClosing();
                Inner.Dispose();
            }
            base.Dispose(disposing);
        }

        private void NotifyClosing()
        {
            _stats.ConnectionDisposed(_connectionId);
        }



        public DbConnection InnerConnection
        {
            get { return Inner; }
        }

        public Guid ConnectionId
        {
            get { return _connectionId; }
        }

        private void OnDtcTransactionCompleted(object sender, TransactionEventArgs args)
        {
            TransactionStatus aborted;
            try
            {
                aborted = args.Transaction.TransactionInformation.Status;
            }
            catch (ObjectDisposedException)
            {
                aborted = TransactionStatus.Aborted;
            }
            _stats.DtcTransactionCompleted(_connectionId, aborted);
        }
    }
}
