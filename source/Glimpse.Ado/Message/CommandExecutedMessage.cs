using System;
using System.Collections.Generic;

namespace Glimpse.Ado.Message
{
    public class CommandExecutedMessage : AdoCommandMessage
    {      
        public CommandExecutedMessage(Guid connectionId, Guid commandId, string commandText, IList<CommandExecutedParamater> parameters, bool hasTransaction)
            : this(connectionId, commandId, commandText, parameters, hasTransaction, false)
        {
        }

        public CommandExecutedMessage(Guid connectionId, Guid commandId, string commandText, IList<CommandExecutedParamater> parameters, bool hasTransaction, bool isAsync) 
            : base(connectionId, commandId)
        {
            CommandId = commandId;
            CommandText = commandText;
            Parameters = parameters;
            HasTransaction = hasTransaction;
            IsAsync = isAsync;
        }

        public string CommandText { get; protected set; }

        public IList<CommandExecutedParamater> Parameters { get; protected set; }

        public bool HasTransaction { get; protected set; }

        public bool IsAsync { get; set; }
    }
}