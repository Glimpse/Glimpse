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

        public void RegisterStart(DateTime dateTime)
        {
            StartDateTime = dateTime;
            StartCount++;
        }

        public void RegisterEnd(DateTime dateTime)
        {
            EndDateTime = dateTime;
            EndCount++;
        }

        public void RegiserCommand(CommandMetadata command)
        {
            Commands.Add(command.Id, command); 
        }

        public void RegiserTransactionStart(TransactionMetadata transaction)
        { 
            Transactions.Add(transaction.Id, transaction);

            var command = Commands.FirstOrDefault(x => x.Value.StartDateTime >= transaction.StartDateTime);
            command.Value.HeadTransaction = transaction;
        }

        public void RegiserTransactionEnd(TransactionMetadata transaction)
        {
            var command = Commands.LastOrDefault(x => x.Value.EndDateTime <= transaction.EndDateTime);
            command.Value.TailTransaction = transaction;
        }

        public string Id { get; private set; } 
        public DateTime? StartDateTime { get; private set; }
        public int StartCount { get; private set; }
        public DateTime? EndDateTime { get; private set; }
        public int EndCount { get; private set; }
        public IDictionary<string, CommandMetadata> Commands { get; private set; }
        public IDictionary<string, TransactionMetadata> Transactions { get; private set; } 
    }
}