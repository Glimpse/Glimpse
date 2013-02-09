using System;
using System.Collections.Generic;

namespace Glimpse.Ado.Plumbing.Models
{
    public class GlimpseDbQueryCommandMetadata 
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
        public int ExecutionIndex { get; set; }
        public GlimpseDbQueryTransactionMetadata HeadTransaction { get; set; }
        public GlimpseDbQueryTransactionMetadata TailTransaction { get; set; }
    }
}