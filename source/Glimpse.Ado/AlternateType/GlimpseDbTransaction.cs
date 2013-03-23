using System;
using System.Data;
using System.Data.Common;
using Glimpse.Ado.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Ado.AlternateType
{
    internal class GlimpseDbTransaction : DbTransaction
    {
        private IMessageBroker messageBroker;

        public GlimpseDbTransaction(DbTransaction transaction, GlimpseDbConnection connection)
        {
            InnerTransaction = transaction; 
            InnerConnection = connection;
            TransactionId = Guid.NewGuid();

            if (MessageBroker != null)
            {
                MessageBroker.Publish(new TransactionBeganMessage(connection.ConnectionId, TransactionId, transaction.IsolationLevel));
            }
        }

        public GlimpseDbTransaction(DbTransaction transaction, GlimpseDbConnection connection, IMessageBroker messageBroker)
            : this(transaction, connection)
        {
            MessageBroker = messageBroker;
        }
         
        private GlimpseDbConnection InnerConnection { get; set; } 

        private IMessageBroker MessageBroker
        {
            get { return messageBroker ?? (messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker()); }
            set { messageBroker = value; }
        }

        protected override DbConnection DbConnection
        {
            get { return InnerConnection; }
        }

        public override IsolationLevel IsolationLevel
        {
            get { return InnerTransaction.IsolationLevel; }
        }


        public override void Commit()
        {
            InnerTransaction.Commit();
            if (MessageBroker != null)
            {
                MessageBroker.Publish(new TransactionCommitMessage(InnerConnection.ConnectionId, TransactionId));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InnerTransaction.Dispose(); 
            }
            base.Dispose(disposing);
        }

        public override void Rollback()
        {
            InnerTransaction.Rollback();

            if (MessageBroker != null)
            {
                MessageBroker.Publish(new TransactionRollbackMessage(InnerConnection.ConnectionId, TransactionId));
            }
        }


        public DbTransaction InnerTransaction { get; set; }        

        public Guid TransactionId { get; set; }
    }
}
