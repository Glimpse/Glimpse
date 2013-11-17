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

        /// <summary>
        /// Gets or sets the id of the command.
        /// </summary>
        /// <value>The command id.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets connection id related with the command.
        /// </summary>
        /// <value>The connection id.</value>
        public string ConnectionId { get; private set; }

        /// <summary>
        /// Gets or sets the actual command.
        /// </summary>
        /// <value>The command.</value>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the exception related to the command.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the time when the command started.
        /// </summary>
        /// <value>The start date time.</value>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time when the command ended.
        /// </summary>
        /// <value>The end date time.</value>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the total number of records that were
        /// affected by the command.
        /// </summary>
        /// <value>The records affected.</value>
        public long? RecordsAffected { get; set; }

        /// <summary>
        /// Gets or sets the total number of records that 
        /// were returned by the command.
        /// </summary>
        /// <value>The total records.</value>
        public long? TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the duration of how long the command
        /// took to execute.
        /// </summary>
        /// <value>The duration value.</value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the time offset of went the command
        /// happened.
        /// </summary>
        /// <value>The offset value.</value>
        public TimeSpan Offset { get; set; }

        /// <summary>
        /// Gets the parameter values for the command.
        /// </summary>
        /// <value>The parameters value.</value>
        public IList<CommandParameterMetadata> Parameters { get; private set; }

        /// <summary>
        /// Gets or sets whether the index of when the command 
        /// was executed.
        /// </summary>
        /// <value>The execution index value.</value>
        public int ExecutionIndex { get; set; }

        /// <summary>
        /// Gets or sets whether the command has a transaction
        /// associated with it.
        /// </summary>
        /// <value>The has transaction value.</value>
        public bool HasTransaction { get; set; }

        /// <summary>
        /// Gets or sets whether the command is a duplicate command.
        /// </summary>
        /// <value>The duplicate value.</value>
        public bool IsDuplicate { get; set; }

        /// <summary>
        /// Gets or sets whether the command was executed through
        /// the async API.
        /// </summary>
        /// <value>The is async value.</value>
        public bool IsAsync { get; set; }
         
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
    }
}