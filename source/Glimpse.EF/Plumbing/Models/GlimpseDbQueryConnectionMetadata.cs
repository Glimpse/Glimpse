using System;
using System.Collections.Generic;

namespace Glimpse.EF.Plumbing.Models
{
    internal class GlimpseDbQueryConnectionMetadata
    {
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

        public string Id { get; private set; }
        public DateTime? StartDateTime { get; private set; }
        public int StartCount { get; private set; }
        public DateTime? EndDateTime { get; private set; }
        public int EndCount { get; private set; }
        public IDictionary<string, GlimpseDbQueryCommandMetadata> Commands { get; private set; }
        public IDictionary<string, GlimpseDbQueryTransactionMetadata> Transactions { get; private set; }
    }
}