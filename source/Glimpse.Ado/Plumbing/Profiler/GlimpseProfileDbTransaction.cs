using System;
using System.Data;
using System.Data.Common;
using Glimpse.Ado.Messages;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.Plumbing.Profiler
{
    internal class GlimpseProfileDbTransaction : DbTransaction
    {
        public GlimpseProfileDbTransaction(DbTransaction transaction, IPipelineInspectorContext inspectorContext, ProviderStats stats, GlimpseProfileDbConnection connection)
        {
            InnerTransaction = transaction;
            InspectorContext = inspectorContext;
            InnerConnection = connection;
            Stats = stats;
            TransactionId = Guid.NewGuid();

            Stats.TransactionBegan(connection.ConnectionId, TransactionId, transaction.IsolationLevel);
            InspectorContext.MessageBroker.Publish(new TransactionBeganMessage(connection.ConnectionId, TransactionId, transaction.IsolationLevel));
        }


        private GlimpseProfileDbConnection InnerConnection { get; set; }
        private ProviderStats Stats { get; set; }
        private IPipelineInspectorContext InspectorContext { get; set; }


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
            Stats.TransactionCommit(InnerConnection.ConnectionId, TransactionId);
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
            Stats.TransactionRolledBack(InnerConnection.ConnectionId, TransactionId);
            InspectorContext.MessageBroker.Publish(new TransactionRollbackMessage(InnerConnection.ConnectionId, TransactionId));
        }


        public DbTransaction InnerTransaction { get; set; }        

        public Guid TransactionId { get; set; }
    }
}
