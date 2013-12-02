using System;
using System.Transactions;

namespace Glimpse.Ado.Message
{
    public class DtcTransactionCompletedMessage : AdoMessage
    {
        public DtcTransactionCompletedMessage(Guid connectionId, TransactionStatus transactionStatus) : base(connectionId)
        {
            Status = transactionStatus;
        }

        public TransactionStatus Status { get; protected set; }
    }
}