using System;
using System.Diagnostics;

namespace Glimpse.Ado.Message
{
    public class CommandStackTraceMessage : AdoCommandPayloadMessage<string>
    {
        public CommandStackTraceMessage(Guid connectionId, Guid commandId, string stackTraceText, bool isAsync = false)
            : base(connectionId, commandId, stackTraceText, isAsync)
        {
        }
    }
}