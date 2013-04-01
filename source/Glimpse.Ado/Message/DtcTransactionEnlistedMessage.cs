using System;
using System.Transactions;

namespace Glimpse.Ado.Message
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