using System.Collections.Generic;
using System.Linq;
using Glimpse.Ado.Message;

namespace Glimpse.Ado.Model
{
    public class MessageAggregator
    {
        private IList<AdoMessage> Messages { get; set; }
        private QueryMetadata Metadata { get; set; }

        public MessageAggregator(IList<AdoMessage> messages)
        {
            Messages = messages;           
        }

        public QueryMetadata Aggregate()
        {
            Metadata = new QueryMetadata();

            AggregateConnectionStart();
            AggregateConnectionClosed();
            AggregateCommandErrors();
            AggregateCommandDurations();
            AggregateCommandExecuted();
            AggregateCommandRowCounts();
            AggregateTransactionBegan();
            AggregateTransactionEnd();

            return Metadata;
        }

        private void AggregateCommandRowCounts()
        {
            var messages = Messages.OfType<CommandRowCountMessage>();
            foreach (var message in messages)
            {
                var command = GetOrCreateCommandFor(message);
                command.TotalRecords = message.RowCount;
            }
        }

        private void AggregateCommandExecuted()
        {
            var messages = Messages.OfType<CommandExecutedMessage>();
            foreach(var message in messages)
            {
                var command = GetOrCreateCommandFor(message);
                command.Command = message.CommandText;
                command.StartDateTime = message.TimeStamp;
                if(message.Parameters != null)
                {
                    foreach (var parameter in message.Parameters)
                    {
                        var parameterMetadata = new CommandParameterMetadata
                        {
                            Name = parameter.Name,
                            Value = parameter.Value,
                            Type = parameter.Type,
                            Size = parameter.Size
                        };
                        command.Parameters.Add(parameterMetadata);
                    }
                }
            }
        }

        private void AggregateCommandDurations()
        {
            var messages = Messages.OfType<CommandDurationAndRowCountMessage>();
            foreach(var message in messages)
            {
                var command = GetOrCreateCommandFor(message);
                command.Elapsed = message.Elapsed;
                command.RecordsAffected = message.RecordsAffected;
                command.EndDateTime = message.TimeStamp;
            }
        }

        private void AggregateCommandErrors()
        {
            var messages = Messages.OfType<CommandErrorMessage>();
            foreach(var message in messages)
            {
                var command = GetOrCreateCommandFor(message);
                command.Elapsed = message.Elapsed;
                command.Exception = message.Exception;
                command.EndDateTime = message.TimeStamp;
            }
        }

        private void AggregateTransactionEnd()
        {
            var commitMessages = Messages.OfType<TransactionCommitMessage>();
            foreach(var message in commitMessages)
            {
                var transaction = GetOrCreateTransactionFor(message);
                transaction.Committed = true;
                transaction.EndDateTime = message.TimeStamp;

                var connection = GetOrCreateConnectionFor(message);
                connection.RegiserTransactionEnd(transaction);
            }

            var rollbackMessages = Messages.OfType<TransactionRollbackMessage>();
            foreach(var message in rollbackMessages)
            {
                var transaction = GetOrCreateTransactionFor(message);
                transaction.Committed = false;
                transaction.EndDateTime = message.TimeStamp;

                var connection = GetOrCreateConnectionFor(message);
                connection.RegiserTransactionEnd(transaction);
            }
        }

        private void AggregateTransactionBegan()
        {
            foreach (var transactionBeginMessage in Messages.OfType<TransactionBeganMessage>())
            {
                var transaction = GetOrCreateTransactionFor(transactionBeginMessage);
                transaction.IsolationLevel = transactionBeginMessage.IsolationLevel.ToString();
                transaction.StartDateTime = transactionBeginMessage.TimeStamp;

                var connection = GetOrCreateConnectionFor(transactionBeginMessage);
                connection.RegiserTransactionStart(transaction);
            }
        }

        private void AggregateConnectionClosed()
        {
            foreach (var connectionClosedMessage in Messages.OfType<ConnectionClosedMessage>())
            {
                var connectionMetadata = GetOrCreateConnectionFor(connectionClosedMessage);
                connectionMetadata.RegisterEnd(connectionClosedMessage.TimeStamp);
            }
        }

        private void AggregateConnectionStart()
        {
            foreach (var connectionStartedMessage in Messages.OfType<ConnectionStartedMessage>())
            {
                var connectionMetadata = GetOrCreateConnectionFor(connectionStartedMessage);
                connectionMetadata.RegisterStart(connectionStartedMessage.TimeStamp);
            }
        }

        protected ConnectionMetadata GetOrCreateConnectionFor(AdoMessage message)
        {            
            ConnectionMetadata connection;
            var connectionId = message.ConnectionId.ToString();

            if (!Metadata.Connections.TryGetValue(connectionId, out connection))
            {
                connection = new ConnectionMetadata(connectionId);
                Metadata.Connections.Add(connectionId, connection);
            }

            return connection;
        }

        protected CommandMetadata GetOrCreateCommandFor(AdoCommandMessage message)
        {            
            CommandMetadata command;
            var connectionId = message.ConnectionId.ToString();
            var commandId = message.CommandId.ToString();

            if (!Metadata.Commands.TryGetValue(commandId, out command))
            {
                command = new CommandMetadata(commandId, connectionId);
                Metadata.Commands.Add(commandId, command);

                var connection = GetOrCreateConnectionFor(message);
                connection.RegiserCommand(command);
            }
            return command;
        }

        protected TransactionMetadata GetOrCreateTransactionFor(AdoTransactionMessage message)
        {
            TransactionMetadata transaction;
            var connectionId = message.ConnectionId.ToString();
            var transactionId = message.TransactionId.ToString();

            if (!Metadata.Transactions.TryGetValue(transactionId, out transaction))
            {
                transaction = new TransactionMetadata(transactionId, connectionId);
                Metadata.Transactions.Add(transactionId, transaction);
            }
            return transaction;
        }
    }
}