using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensions;

namespace Glimpse.Ado.Plumbing.Models
{
    public class GlimpseDbQueryConnectionMetadata
    {
        private static readonly KeyValuePair<string, GlimpseDbQueryCommandMetadata> DefaultCommandKey = default(KeyValuePair<string, GlimpseDbQueryCommandMetadata>);
        private GlimpseDbQueryTransactionMetadata TempTransaction { get; set; }

        public GlimpseDbQueryConnectionMetadata(string id)
        {
            Id = id;
            Commands = new Dictionary<string, GlimpseDbQueryCommandMetadata>();
            Transactions = new Dictionary<string, GlimpseDbQueryTransactionMetadata>(); 
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

        public void RegiserCommand(GlimpseDbQueryCommandMetadata command)
        {
            Commands.Add(command.Id, command);

            if (TempTransaction != null)
            {
                command.HeadTransaction = TempTransaction;
                TempTransaction = null;
            }
        }

        public void RegiserTransactionStart(GlimpseDbQueryTransactionMetadata transaction)
        {
            TempTransaction = transaction; 
        }

        public void RegiserTransactionEnd(GlimpseDbQueryTransactionMetadata transaction)
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
        public IDictionary<string, GlimpseDbQueryCommandMetadata> Commands { get; private set; }
        public IDictionary<string, GlimpseDbQueryTransactionMetadata> Transactions { get; private set; }
        public TimeSpan? EllapsedMilliseconds
        {
            get
            {
                if (!StartDateTime.HasValue || !EndDateTime.HasValue)
                {
                    return null;
                }
                 
                return EndDateTime.Value - StartDateTime.Value;
            }
        }
    }
}