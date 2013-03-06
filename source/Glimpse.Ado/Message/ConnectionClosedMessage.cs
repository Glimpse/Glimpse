using System;

namespace Glimpse.Ado.Message
{
    public class ConnectionClosedMessage : AdoMessage
    {
        public ConnectionClosedMessage(Guid connectionId) : base(connectionId)
        {
        }
    }
}