using System;
using System.Linq;
using Glimpse.Ado.Model;

namespace Glimpse.Ado
{
    internal static class SqlStatisticsCalculator
    {
        public static SqlStatistics Caluculate(QueryMetadata queryMetadata)
        {
            var queryCount = queryMetadata.Commands.Count;
            var connectionCount = queryMetadata.Connections.Count;
            var transactionCount = queryMetadata.Transactions.Count;

            var queryExecutionTime = new TimeSpan();
            var connectionOpenTime = new TimeSpan();

            queryExecutionTime = queryMetadata.Commands.Aggregate(queryExecutionTime, (totalDuration, command) => totalDuration + command.Value.Duration);
            connectionOpenTime = queryMetadata.Connections.Aggregate(connectionOpenTime, (totalDuration, connection) => totalDuration + connection.Value.Duration.GetValueOrDefault(TimeSpan.Zero));

            return new SqlStatistics(
                queryCount,
                connectionCount,
                transactionCount,
                queryExecutionTime,
                connectionOpenTime);
        }
    }
}
