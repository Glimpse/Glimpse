using System;

namespace Glimpse.Ado.Messages
{
    public class ConnectionClosedMessage : AdoMessage
    {
        public ConnectionClosedMessage(Guid connectionId) : base(connectionId)
        {
        }
    }
}