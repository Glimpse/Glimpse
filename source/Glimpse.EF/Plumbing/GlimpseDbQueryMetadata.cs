using System;
using System.Collections.Generic;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseDbQueryMetadata
    {
        public GlimpseDbQueryMetadata()
        {
            Connections = new Dictionary<string, GlimpseDbQueryConnectionMetadata>();
            Commands = new Dictionary<string, GlimpseDbQueryCommandMetadata>();
            Transactions = new Dictionary<string, GlimpseDbQueryTransactionMetadata>(); 
        }

        public IDictionary<string, GlimpseDbQueryConnectionMetadata> Connections { get; private set; }
        public IDictionary<string, GlimpseDbQueryCommandMetadata> Commands { get; private set; }
        public IDictionary<string, GlimpseDbQueryTransactionMetadata> Transactions { get; private set; }

        public IList<string> Warnings { get; private set; }
    }

    internal class GlimpseDbQueryCommandMetadata
    {
        public GlimpseDbQueryCommandMetadata(string id, string connectionId)
        {
            Id = id;
            ConnectionId = connectionId;
            Parameters = new List<GlimpseDbQueryCommandParameterMetadata>(); 
        }

        public string Id { get; private set; }
        public string ConnectionId { get; private set; }
        public string Command { get; set; }
        public Exception Exception { get; set; }
        public DateTime? ExecutedDateTime { get; set; }
        public int? RecordsAffected { get; set; }
        public int? TotalRecords { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public IList<GlimpseDbQueryCommandParameterMetadata> Parameters { get; private set; }
    }

    internal class GlimpseDbQueryTransactionMetadata
    {
    }

    internal class GlimpseDbQueryCommandParameterMetadata
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
    }
}
