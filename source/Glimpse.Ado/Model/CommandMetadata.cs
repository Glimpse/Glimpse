using System;
using System.Collections.Generic;

namespace Glimpse.Ado.Model
{
    /// <summary>
    /// Class CommandMetadata
    /// </summary>
    public class CommandMetadata 
    {
        public CommandMetadata(string id, string connectionId)
        {
            Id = id;
            ConnectionId = connectionId;
            Parameters = new List<CommandParameterMetadata>(); 
        }

        public string Id { get; private set; } 
        public string ConnectionId { get; private set; }
        public string Command { get; set; }
        public Exception Exception { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public long? RecordsAffected { get; set; }
        public long? TotalRecords { get; set; }
        public TimeSpan Elapsed { get; set; }
        public IList<CommandParameterMetadata> Parameters { get; private set; }
        public int ExecutionIndex { get; set; }
         
        /// <summary>
        /// Gets or sets the head transaction. Set when the command 
        /// represents the first command in the transaction scope.
        /// </summary>
        /// <value>The head transaction.</value>
        public TransactionMetadata HeadTransaction { get; set; }

        /// <summary>
        /// Gets or sets the tail transaction. Set when the command 
        /// represents the last command in the transaction scope.
        /// </summary>
        /// <value>The tail transaction.</value>
        public TransactionMetadata TailTransaction { get; set; }

        //public TimeSpan Elapsed { get; set; }
    }
}