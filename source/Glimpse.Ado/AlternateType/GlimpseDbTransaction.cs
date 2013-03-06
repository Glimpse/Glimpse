using System;
using System.Data;
using System.Data.Common;
using Glimpse.Ado.Message;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.AlternateType
{
    internal class GlimpseDbTransaction : DbTransaction
    {
        public GlimpseDbTransaction(DbTransaction transaction, IInspectorContext inspectorContext, GlimpseDbConnection connection)
        {
            InnerTransaction = transaction;
            InspectorContext = inspectorContext;
            InnerConnection = connection;
            TransactionId = Guid.NewGuid();

            InspectorContext.MessageBroker.Publish(new TransactionBeganMessage(connection.ConnectionId, TransactionId, transaction.IsolationLevel));
        }


        private GlimpseDbConnection InnerConnection { get; set; }
        private IInspectorContext InspectorContext { get; set; }

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
            InspectorContext.MessageBroker.Publish(new TransactionCommitMessage(InnerConnection.ConnectionId, TransactionId));
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
            InspectorContext.MessageBroker.Publish(new TransactionRollbackMessage(InnerConnection.ConnectionId, TransactionId));
        }


        public DbTransaction InnerTransaction { get; set; }        

        public Guid TransactionId { get; set; }
    }
}
