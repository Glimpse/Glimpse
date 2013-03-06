using System;
using System.Collections.Generic;

namespace Glimpse.Ado.Message
{
    public class CommandExecutedMessage : AdoCommandMessage
    {      
        public string CommandText { get; protected set; }
        public IList<Tuple<string, object, string, int>> Parameters { get; protected set; }

        public CommandExecutedMessage(Guid connectionId, Guid commandId, string commandText,  IList<Tuple<string, object, string, int>> parameters) 
            : base(connectionId, commandId)
        {
            CommandId = commandId;
            CommandText = commandText;
            Parameters = parameters;
        }
    }
}