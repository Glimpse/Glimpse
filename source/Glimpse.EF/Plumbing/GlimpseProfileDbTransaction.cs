using System.Data;
using System.Data.Common;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbTransaction : DbTransaction
    { 
        private readonly DbTransaction _inner;
        private readonly ProviderStats _stats;
        private readonly GlimpseProfileDbConnection _connection;

        public GlimpseProfileDbTransaction(DbTransaction inner, ProviderStats stats, GlimpseProfileDbConnection connection)
        {
            _inner = inner;
            _stats = stats; 
            _connection = connection;

            _stats.TransactionBegan(connection.ConnectionId, inner.IsolationLevel);
        }


        protected override DbConnection DbConnection
        {
            get { return _connection; }
        }

        public override IsolationLevel IsolationLevel
        {
            get { return _inner.IsolationLevel; }
        }


        public override void Commit()
        {
            _inner.Commit(); 
            _stats.TransactionCommit(_connection.ConnectionId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose(); 
                _stats.TransactionDisposed(_connection.ConnectionId);
            }
            base.Dispose(disposing);
        }

        public override void Rollback()
        {
            _inner.Rollback(); 
            _stats.TransactionRolledBack(_connection.ConnectionId);
        }


        public DbTransaction Inner
        {
            get { return _inner; }
        } 
    }
}
