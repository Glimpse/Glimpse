using System;
using System.Transactions;

namespace Glimpse.Ado.Message
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