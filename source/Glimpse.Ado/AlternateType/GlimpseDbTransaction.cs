using System;
using System.Data;
using System.Data.Common;
using Glimpse.Ado.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;

namespace Glimpse.Ado.AlternateType
{
    public class GlimpseDbTransaction : DbTransaction
    {
        private readonly TimeSpan timerTimeSpan;
        private IMessageBroker messageBroker; 
        private IExecutionTimer timerStrategy; 

        public GlimpseDbTransaction(DbTransaction transaction, GlimpseDbConnection connection)
        {
            InnerTransaction = transaction; 
            InnerConnection = connection;
            TransactionId = Guid.NewGuid();

            if (MessageBroker != null && TimerStrategy != null)
            {
                timerTimeSpan = TimerStrategy.Start();
                
                MessageBroker.Publish(
                    new TransactionBeganMessage(connection.ConnectionId, TransactionId, transaction.IsolationLevel)
                    .AsTimedMessage(timerTimeSpan));
            }
        }

        public GlimpseDbTransaction(DbTransaction transaction, GlimpseDbConnection connection, IMessageBroker messageBroker, IExecutionTimer timerStrategy)
            : this(transaction, connection)
        {
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
        }

        public GlimpseDbConnection InnerConnection { get; set; } 
         
        public DbTransaction InnerTransaction { get; set; }

        public Guid TransactionId { get; set; }

        public override IsolationLevel IsolationLevel
        {
            get { return InnerTransaction.IsolationLevel; }
        }

        protected override DbConnection DbConnection
        {
            get { return InnerConnection; }
        }

        private IMessageBroker MessageBroker
        {
            get { return messageBroker ?? (messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker()); }
            set { messageBroker = value; }
        }

        private IExecutionTimer TimerStrategy
        {
            get { return timerStrategy ?? (timerStrategy = GlimpseConfiguration.GetConfiguredTimerStrategy()()); }
            set { timerStrategy = value; }
        }

        public override void Commit()
        {
            InnerTransaction.Commit();

            if (MessageBroker != null && TimerStrategy != null)
            { 
                MessageBroker.Publish(
                    new TransactionCommitMessage(InnerConnection.ConnectionId, TransactionId)
                    .AsTimedMessage(TimerStrategy.Stop(timerTimeSpan)));
            }
        }

        public override void Rollback()
        {
            InnerTransaction.Rollback();

            if (MessageBroker != null && TimerStrategy != null)
            {
                MessageBroker.Publish(
                    new TransactionRollbackMessage(InnerConnection.ConnectionId, TransactionId)
                    .AsTimedMessage(TimerStrategy.Stop(timerTimeSpan)));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InnerTransaction.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
