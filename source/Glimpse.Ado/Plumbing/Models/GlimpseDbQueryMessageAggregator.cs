using System.Collections.Generic;
using System.Linq;
using Glimpse.Ado.Messages;

namespace Glimpse.Ado.Plumbing.Models
{
    public class GlimpseDbQueryMessageAggregator
    {
        private IList<AdoMessage> Messages { get; set; }
        private GlimpseDbQueryMetadata Metadata { get; set; }

        public GlimpseDbQueryMessageAggregator(IList<AdoMessage> messages)
        {
            Messages = messages;           
        }

        public GlimpseDbQueryMetadata Aggregate()
        {
            Metadata = new GlimpseDbQueryMetadata();

            AggregateConnectionStart();
            AggregateConnectionClosed();
            AggregateTransactionBegan();
            AggregateTransactionEnd();
            AggregateCommandErrors();
            AggregateCommandDurations();
            AggregateCommandExecuted();
            AggregateCommandRowCounts();

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
                if(message.Parameters != null)
                {
                    foreach (var parameter in message.Parameters)
                    {
                        var parameterMetadata = new GlimpseDbQueryCommandParameterMetadata
                        {
                            Name = parameter.Item1,
                            Value = parameter.Item2,
                            Type = parameter.Item3,
                            Size = parameter.Item4
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
                command.ElapsedMilliseconds = message.ElapsedMilliseconds;
                command.RecordsAffected = message.RecordsAffected;
            }
        }

        private void AggregateCommandErrors()
        {
            var messages = Messages.OfType<CommandErrorMessage>();
            foreach(var message in messages)
            {
                var command = GetOrCreateCommandFor(message);
                command.Exception = message.Exception;
            }
        }

        private void AggregateTransactionEnd()
        {
            var commitMessages = Messages.OfType<TransactionCommitMessage>();
            foreach(var message in commitMessages)
            {
                var connection = GetOrCreateConnectionFor(message);
                var transaction = GetOrCreateTransactionFor(message);
                transaction.Committed = true;
                connection.RegiserTransactionEnd(transaction);
            }

            var rollbackMessages = Messages.OfType<TransactionRollbackMessage>();
            foreach(var message in rollbackMessages)
            {
                var connection = GetOrCreateConnectionFor(message);
                var transaction = GetOrCreateTransactionFor(message);
                transaction.Committed = false;
                connection.RegiserTransactionEnd(transaction);
            }
        }

        private void AggregateTransactionBegan()
        {
            foreach (var transactionBeginMessage in Messages.OfType<TransactionBeganMessage>())
            {
                var transaction = GetOrCreateTransactionFor(transactionBeginMessage);
                transaction.IsolationLevel = transactionBeginMessage.IsolationLevel.ToString();

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

        protected GlimpseDbQueryConnectionMetadata GetOrCreateConnectionFor(AdoMessage message)
        {            
            GlimpseDbQueryConnectionMetadata connection;
            var connectionId = message.ConnectionId.ToString();

            if (!Metadata.Connections.TryGetValue(connectionId, out connection))
            {
                connection = new GlimpseDbQueryConnectionMetadata(connectionId);
                Metadata.Connections.Add(connectionId, connection);
            }

            return connection;
        }

        protected GlimpseDbQueryCommandMetadata GetOrCreateCommandFor(AdoCommandMessage message)
        {            
            GlimpseDbQueryCommandMetadata command;
            var connectionId = message.ConnectionId.ToString();
            var commandId = message.CommandId.ToString();

            if (!Metadata.Commands.TryGetValue(commandId, out command))
            {
                command = new GlimpseDbQueryCommandMetadata(commandId, connectionId);
                Metadata.Commands.Add(commandId, command);

                var connection = GetOrCreateConnectionFor(message);
                connection.RegiserCommand(command);
            }
            return command;
        }

        protected GlimpseDbQueryTransactionMetadata GetOrCreateTransactionFor(AdoTransactionMessage message)
        {
            GlimpseDbQueryTransactionMetadata transaction;
            var connectionId = message.ConnectionId.ToString();
            var transactionId = message.TransactionId.ToString();

            if (!Metadata.Transactions.TryGetValue(transactionId, out transaction))
            {
                transaction = new GlimpseDbQueryTransactionMetadata(transactionId, connectionId);
                Metadata.Transactions.Add(transactionId, transaction);
            }
            return transaction;
        }
    }
}