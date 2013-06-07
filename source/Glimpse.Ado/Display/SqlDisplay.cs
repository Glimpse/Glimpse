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

            var queryCount = queryData.Commands.Count;
            var connectionCount = queryData.Connections.Count;
            var transactionCount = queryData.Transactions.Count;

            var queryExecutionTime = new TimeSpan();
            var connectionOpenTime = new TimeSpan();

            foreach (var command in queryData.Commands)
            {
                var commandMetadata = command.Value;
                queryExecutionTime += commandMetadata.Duration;
            }

            foreach (var connection in queryData.Connections)
            {
                var connectionMetadata = connection.Value;
                connectionOpenTime += connectionMetadata.Duration.GetValueOrDefault(TimeSpan.Zero);
            }

            return new
                {
                    queryCount,
                    connectionCount,
                    transactionCount,
                    queryExecutionTime,
                    connectionOpenTime
                };
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<AdoMessage>();
        }
    }
}