using System;

namespace Glimpse.Ado.Message
{
    public class ConnectionStartedMessage : AdoMessage
    {
        public ConnectionStartedMessage(Guid connectionId) : base(connectionId)
        {
        }
    }
}