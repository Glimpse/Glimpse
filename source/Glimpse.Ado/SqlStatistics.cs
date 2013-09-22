using System;

namespace Glimpse.Ado
{
    internal class SqlStatistics
    {
        public SqlStatistics(int queryCount, int connectionCount, int transactionCount, TimeSpan queryExecutionTime, TimeSpan connectionOpenTime)
        {
            this.QueryCount = queryCount;
            this.ConnectionCount = connectionCount;
            this.TransactionCount = transactionCount;
            this.QueryExecutionTime = queryExecutionTime;
            this.ConnectionOpenTime = connectionOpenTime;
        }

        public int QueryCount { get; private set; }
        public int ConnectionCount { get; private set; }
        public int TransactionCount { get; private set; }
        public TimeSpan QueryExecutionTime { get; private set; }
        public TimeSpan ConnectionOpenTime { get; private set; }
    }
}
