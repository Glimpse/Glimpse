using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Transactions;
using Glimpse.Ado.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;

namespace Glimpse.Ado.AlternateType
{
    public class GlimpseDbConnection : DbConnection
    {
        private IMessageBroker messageBroker;
        private IExecutionTimer timerStrategy; 
        private TimeSpan timerTimeSpan;

        public GlimpseDbConnection(DbConnection connection)
            : this(connection, connection.TryGetProfiledProviderFactory())
        { 
        }

        public GlimpseDbConnection(DbConnection connection, DbProviderFactory providerFactory)
            : this(connection, providerFactory, Guid.NewGuid())
        { 
        }

        public GlimpseDbConnection(DbConnection connection, DbProviderFactory providerFactory, Guid connectionId)
        {
            InnerConnection = connection;
            InnerProviderFactory = providerFactory;
            ConnectionId = connectionId;

            if (MessageBroker != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    OpenConnection();
                }

                connection.StateChange += StateChangeHaneler;               
            }
        }

        public GlimpseDbConnection(DbConnection connection, DbProviderFactory providerFactory, Guid connectionId, IMessageBroker messageBroker, IExecutionTimer timerStrategy)
            : this(connection, providerFactory, connectionId)
        {
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
        }

        public DbProviderFactory InnerProviderFactory { get; set; }

        public DbConnection InnerConnection { get; set; }

        public Guid ConnectionId { get; set; }

        private IMessageBroker MessageBroker
        {
            get { return messageBroker ?? (messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker()); }
            set { messageBroker = value; }
        }

        private IExecutionTimer TimerStrategy
        {
            get { return timerStrategy ?? (timerStrategy = GlimpseConfiguration.GetConfiguredTimerStrategy()()); }
            set { timerStrategy = value; }
        }

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
                if (MessageBroker != null)
                {
                    MessageBroker.Publish(new DtcTransactionEnlistedMessage(ConnectionId, transaction.IsolationLevel));
                }
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
            return new GlimpseDbTransaction(InnerConnection.BeginTransaction(isolationLevel), this);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new GlimpseDbCommand(InnerConnection.CreateCommand(), this);
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
                InnerConnection.StateChange -= StateChangeHaneler;
            }
            InnerConnection = null;
            InnerProviderFactory = null;
            base.Dispose(disposing);
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

            if (MessageBroker != null)
            {
                MessageBroker.Publish(new DtcTransactionCompletedMessage(ConnectionId, aborted));
            }
        }

        private void StateChangeHaneler(object sender, StateChangeEventArgs args)
        { 
            if (args.CurrentState == ConnectionState.Open)
            {
                OpenConnection();
            }
            else if (args.CurrentState == ConnectionState.Closed)
            {
                ClosedConnection();
            }
        }

        private void OpenConnection()
        {
            timerTimeSpan = TimerStrategy.Start();

            MessageBroker.Publish(
                new ConnectionStartedMessage(ConnectionId)
                .AsTimedMessage(timerTimeSpan));
        }

        private void ClosedConnection()
        {
            MessageBroker.Publish(
                new ConnectionClosedMessage(ConnectionId)
                .AsTimedMessage(TimerStrategy.Stop(timerTimeSpan)));
        }
    }
}
