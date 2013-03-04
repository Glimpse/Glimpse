using System;
using System.Transactions;

namespace Glimpse.Ado.Messages
{
    public class DtcTransactionEnlistedMessage : AdoMessage
    {
        public IsolationLevel IsolationLevel { get; protected set; }

        public DtcTransactionEnlistedMessage(Guid connectionId, IsolationLevel level) : base(connectionId)
        {
            IsolationLevel = level;
        }
    }
}