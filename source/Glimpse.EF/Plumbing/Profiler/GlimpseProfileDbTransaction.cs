using System.Data;
using System.Data.Common;

namespace Glimpse.EF.Plumbing.Profiler
{
    internal class GlimpseProfileDbTransaction : DbTransaction
    {
        public GlimpseProfileDbTransaction(DbTransaction transaction, ProviderStats stats, GlimpseProfileDbConnection connection)
        {
            InnerTransaction = transaction;
            InnerConnection = connection;
            Stats = stats; 

            Stats.TransactionBegan(connection.ConnectionId, transaction.IsolationLevel);
        }


        private DbTransaction InnerTransaction { get; set; }
        private GlimpseProfileDbConnection InnerConnection { get; set; }
        private ProviderStats Stats { get; set; }


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
            Stats.TransactionCommit(InnerConnection.ConnectionId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InnerTransaction.Dispose(); 
                Stats.TransactionDisposed(InnerConnection.ConnectionId);
            }
            base.Dispose(disposing);
        }

        public override void Rollback()
        {
            InnerTransaction.Rollback(); 
            Stats.TransactionRolledBack(InnerConnection.ConnectionId);
        }


        public DbTransaction Inner
        {
            get { return InnerTransaction; }
        } 
    }
}
