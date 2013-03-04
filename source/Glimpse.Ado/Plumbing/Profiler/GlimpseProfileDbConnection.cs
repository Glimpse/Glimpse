using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Transactions;
using Glimpse.Ado.Messages;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.Plumbing.Profiler
{
    public class GlimpseProfileDbConnection : DbConnection
    {
        public GlimpseProfileDbConnection(
            DbConnection inner, 
            DbProviderFactory providerFactory, 
            IInspectorContext inspectorContext, 
            Guid connectionId)
        {
            InnerConnection = inner;
            InnerProviderFactory = providerFactory;
            ConnectionId = connectionId;
            InspectorContext = inspectorContext;
            InspectorContext.MessageBroker.Publish(new ConnectionStartedMessage(ConnectionId));
        }

        private DbProviderFactory InnerProviderFactory { get; set; } 
        private IInspectorContext InspectorContext { get; set; }

        public override string ConnectionString
        {
            get { return InnerConnection.ConnectionString; }
            set { InnerConnection.ConnectionString = value; }
        }

        public override int ConnectionTimeout
        {
            get { return InnerConnection.ConnectionTimeout; }
        }

        public override string Database
        {
            get { return InnerConnection.Database; }
        }

        public override string DataSource
        {
            get { return InnerConnection.DataSource; }
        }

        protected override DbProviderFactory DbProviderFactory
        {
            get { return InnerProviderFactory; }
        }

        public override ConnectionState State
        {
            get { return InnerConnection.State; }
        }

        public override string ServerVersion
        {
            get { return InnerConnection.ServerVersion; }
        }

        public override ISite Site
        {
            get { return InnerConnection.Site; }
            set { InnerConnection.Site = value; }
        }

        public override event StateChangeEventHandler StateChange 
        {
            add { InnerConnection.StateChange += value; }
            remove { InnerConnection.StateChange -= value; }
        }

        public override void ChangeDatabase(string databaseName)
        {
            InnerConnection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            InnerConnection.Close();
            NotifyClosing();
        }

        public override void Open()
        {
            InnerConnection.Open();
        }

        public override void EnlistTransaction(Transaction transaction)
        {
            InnerConnection.EnlistTransaction(transaction);
            if (transaction != null)
            {
                transaction.TransactionCompleted += OnDtcTransactionCompleted; 
                InspectorContext.MessageBroker.Publish(new DtcTransactionEnlistedMessage(ConnectionId, transaction.IsolationLevel));
            }
        }
         
        public override DataTable GetSchema()
        {
            return InnerConnection.GetSchema();
        }

        public override DataTable GetSchema(string collectionName)
        {
            return InnerConnection.GetSchema(collectionName);
        }

        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            return InnerConnection.GetSchema(collectionName, restrictionValues);
        }



        protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return new GlimpseProfileDbTransaction(InnerConnection.BeginTransaction(isolationLevel), InspectorContext, this);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new GlimpseProfileDbCommand(InnerConnection.CreateCommand(), InspectorContext);
        }

        protected override object GetService(Type service)
        {
            return ((IServiceProvider)InnerConnection).GetService(service);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && InnerConnection != null)
            { 
                InnerConnection.Dispose();
            }
            InnerConnection = null;
            InnerProviderFactory = null;
            base.Dispose(disposing);
        }

        private void NotifyClosing()
        {
            InspectorContext.MessageBroker.Publish(new ConnectionClosedMessage(ConnectionId));
        }

        public DbConnection InnerConnection { get; set; }

        public Guid ConnectionId { get; set; }

        

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
            InspectorContext.MessageBroker.Publish(new DtcTransactionCompletedMessage(ConnectionId, aborted));
        }
    }
}
