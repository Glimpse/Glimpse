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
                command.StartDateTime = message.StartTime;
                command.Offset = message.Offset;
                command.HasTransaction = message.HasTransaction;
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
                command.Duration = message.Duration;
                command.RecordsAffected = message.RecordsAffected;
                command.StartDateTime = message.StartTime; //Reason we set it again is we now have a better time than the start
                command.EndDateTime = message.StartTime + message.Offset;
                command.Offset = message.Offset;
            }
        }

        private void AggregateCommandErrors()
        {
            var messages = Messages.OfType<CommandErrorMessage>();
            foreach(var message in messages)
            {
                var command = GetOrCreateCommandFor(message);
                command.Duration = message.Duration;
                command.Exception = message.Exception;
                command.StartDateTime = message.StartTime; //Reason we set it again is we now have a better time than the start
                command.EndDateTime = message.StartTime + message.Offset;
                command.Offset = message.Offset;
            }
        }

        private void AggregateTransactionEnd()
        {
            var commitMessages = Messages.OfType<TransactionCommitMessage>();
            foreach(var message in commitMessages)
            {
                var transaction = GetOrCreateTransactionFor(message);
                transaction.Committed = true;
                transaction.StartDateTime = message.StartTime; //Reason we set it again is we now have a better time than the start
                transaction.EndDateTime = message.StartTime + message.Offset;
                transaction.Duration = message.Duration;
                transaction.Offset = message.Offset;

                var connection = GetOrCreateConnectionFor(message);
                connection.RegiserTransactionEnd(transaction);
            }

            var rollbackMessages = Messages.OfType<TransactionRollbackMessage>();
            foreach(var message in rollbackMessages)
            {
                var transaction = GetOrCreateTransactionFor(message);
                transaction.Committed = false;
                transaction.StartDateTime = message.StartTime; //Reason we set it again is we now have a better time than the start
                transaction.EndDateTime = message.StartTime + message.Offset;
                transaction.Duration = message.Duration;
                transaction.Offset = message.Offset;

                var connection = GetOrCreateConnectionFor(message);
                connection.RegiserTransactionEnd(transaction);
            }
        }

        private void AggregateTransactionBegan()
        {
            foreach (var message in Messages.OfType<TransactionBeganMessage>())
            {
                var transaction = GetOrCreateTransactionFor(message);
                transaction.IsolationLevel = message.IsolationLevel.ToString();
                transaction.StartDateTime = message.StartTime;
                transaction.Offset = message.Offset;

                var connection = GetOrCreateConnectionFor(message);
                connection.RegiserTransactionStart(transaction);
            }
        }

        private void AggregateConnectionClosed()
        {
            foreach (var message in Messages.OfType<ConnectionClosedMessage>())
            {
                var connection = GetOrCreateConnectionFor(message);
                connection.StartDateTime = message.StartTime; //Reason we set it again is we now have a better time than the start
                connection.EndDateTime = message.StartTime + message.Offset;
                connection.Duration = message.Duration;
                connection.Offset = message.Offset;

                connection.RegisterEnd();
            }
        }

        private void AggregateConnectionStart()
        {
            foreach (var message in Messages.OfType<ConnectionStartedMessage>())
            {
                var connection = GetOrCreateConnectionFor(message);
                connection.StartDateTime = message.StartTime;
                connection.Offset = message.Offset;

                connection.RegisterStart();
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