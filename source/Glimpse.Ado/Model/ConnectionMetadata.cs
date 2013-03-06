using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Ado.Model
{
    public class ConnectionMetadata
    {
        private static readonly KeyValuePair<string, CommandMetadata> DefaultCommandKey = default(KeyValuePair<string, CommandMetadata>);
        private TransactionMetadata TempTransaction { get; set; }

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

            if (TempTransaction != null)
            {
                command.HeadTransaction = TempTransaction;
                TempTransaction = null;
            }
        }

        public void RegiserTransactionStart(TransactionMetadata transaction)
        {
            TempTransaction = transaction; 
        }

        public void RegiserTransactionEnd(TransactionMetadata transaction)
        {
            var last = Commands.LastOrDefault();
            if (!last.Equals(DefaultCommandKey))
                last.Value.TailTransaction = transaction;
            TempTransaction = null;
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