using System;

namespace Glimpse.Ado.Messages
{
    public class TransactionCommitMessage : AdoTransactionMessage
    {
        public TransactionCommitMessage(Guid connectionId, Guid transactionId) : base(connectionId, transactionId)
        {
        }
    }
}