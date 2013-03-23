using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Ado.Model
{
    public class ConnectionMetadata
    { 
        public ConnectionMetadata(string id)
        {
            Id = id;
            Commands = new Dictionary<string, CommandMetadata>();
            Transactions = new Dictionary<string, TransactionMetadata>(); 
        }

        public void RegisterStart()
        {
            StartCount++;
        }

        public void RegisterEnd()
        { 
            EndCount++;
        }

        public void RegiserCommand(CommandMetadata command)
        {
            Commands.Add(command.Id, command); 
        }

        public void RegiserTransactionStart(TransactionMetadata transaction)
        { 
            Transactions.Add(transaction.Id, transaction);

            var command = Commands.FirstOrDefault(x => x.Value.Offset >= transaction.Offset);
            command.Value.HeadTransaction = transaction;
        }

        public void RegiserTransactionEnd(TransactionMetadata transaction)
        {
            var command = Commands.LastOrDefault(x => x.Value.Offset <= transaction.Offset + transaction.Duration);
            command.Value.TailTransaction = transaction;
        }

        public string Id { get; private set; } 
        public DateTime? StartDateTime { get; set; }
        public int StartCount { get; private set; }
        public DateTime? EndDateTime { get; set; }
        public int EndCount { get; private set; }
        public IDictionary<string, CommandMetadata> Commands { get; private set; }
        public IDictionary<string, TransactionMetadata> Transactions { get; private set; }
        public TimeSpan? Duration { get; set; }
        public TimeSpan? Offset { get; set; }
    }
}