using System;
using System.Linq;
using Glimpse.Ado.Message;
using Glimpse.Ado.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Ado.Display
{
    [Obsolete]
    public class SqlDisplay : IDisplay, ITabSetup, IKey
    {
        private const string InternalName = "sql";

        public string Name
        {
            get { return InternalName; }
        }

        public string Key
        {
            get { return InternalName; }
        }

        public object GetData(ITabContext context)
        {
            var messages = context.GetMessages<AdoMessage>().ToList();
            var aggregator = new MessageAggregator(messages);
            var queryData = aggregator.Aggregate();

            SqlStatistics sqlStatistics = SqlStatisticsCalculator.Caluculate(queryData);

            return new
            {
                queryCount = sqlStatistics.QueryCount,
                connectionCount = sqlStatistics.ConnectionCount,
                transactionCount = sqlStatistics.TransactionCount,
                queryExecutionTime = sqlStatistics.QueryExecutionTime,
                connectionOpenTime = sqlStatistics.ConnectionOpenTime
            };
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<AdoMessage>();
        }
    }
}