using System;
using System.Data;
using System.Data.Common;
using Glimpse.Ado.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;

namespace Glimpse.Ado.AlternateType
{
    internal class GlimpseDbTransaction : DbTransaction
    {
        private IMessageBroker messageBroker; 
        private IExecutionTimer timerStrategy; 
        private readonly TimeSpan timerTimeSpan;

        public GlimpseDbTransaction(DbTransaction transaction, GlimpseDbConnection connection)
        {
            InnerTransaction = transaction; 
            InnerConnection = connection;
            TransactionId = Guid.NewGuid();

            if (MessageBroker != null)
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

        protected override DbConnection DbConnection
        {
            get { return InnerConnection; }
        }

        public override IsolationLevel IsolationLevel
        {
            get { return InnerTransaction.IsolationLevel; }
        }

        public override void Commit()
        {
            InnerTransaction.Commit();
            if (MessageBroker != null)
            { 
                MessageBroker.Publish(
                    new TransactionCommitMessage(InnerConnection.ConnectionId, TransactionId)
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

        public override void Rollback()
        {
            InnerTransaction.Rollback();

            if (MessageBroker != null)
            {
                MessageBroker.Publish(
                    new TransactionRollbackMessage(InnerConnection.ConnectionId, TransactionId)
                    .AsTimedMessage(TimerStrategy.Stop(timerTimeSpan)));
            }
        }


        public DbTransaction InnerTransaction { get; set; }        

        public Guid TransactionId { get; set; }
    }
}
