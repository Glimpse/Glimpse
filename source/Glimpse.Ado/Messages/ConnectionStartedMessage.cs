using System;

namespace Glimpse.Ado.Messages
{
    public class ConnectionStartedMessage : AdoMessage
    {
        public ConnectionStartedMessage(Guid connectionId) : base(connectionId)
        {
        }
    }
}