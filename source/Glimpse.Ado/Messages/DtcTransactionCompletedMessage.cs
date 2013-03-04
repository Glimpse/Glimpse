using System;
using System.Transactions;

namespace Glimpse.Ado.Messages
{
    public class DtcTransactionCompletedMessage : AdoMessage
    {
        public TransactionStatus Status { get; protected set; }

        public DtcTransactionCompletedMessage(Guid connectionId, TransactionStatus transactionStatus) : base(connectionId)
        {
            Status = transactionStatus;
        }
    }
}