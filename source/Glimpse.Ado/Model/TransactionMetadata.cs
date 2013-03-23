using System;

namespace Glimpse.Ado.Model
{
    public class TransactionMetadata 
    {
        public TransactionMetadata(string transactionId, string connectionId)
        {
            Id = transactionId;
            ConnectionId = connectionId;
        }

        public string Id { get; private set; } 
        public string ConnectionId { get; set; }  
        public string IsolationLevel { get; set; }
        public int ExecutionIndex { get; set; }
        public bool? Committed { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public TimeSpan? Offset { get; set; }
    }
}