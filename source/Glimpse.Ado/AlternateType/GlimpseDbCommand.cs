using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
#if NET45
using System.Threading;
using System.Threading.Tasks;
#endif
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Ado.AlternateType
{
    public class GlimpseDbCommand : DbCommand
    {
        private IMessageBroker messageBroker;
        private IExecutionTimer timerStrategy; 

        public GlimpseDbCommand(DbCommand innerCommand)
        {
            InnerCommand = innerCommand; 
        }

        public GlimpseDbCommand(DbCommand innerCommand, GlimpseDbConnection connection) 
            : this(innerCommand)
        {
            InnerConnection = connection;
        }

        public GlimpseDbCommand(DbCommand innerCommand, GlimpseDbConnection connection, IMessageBroker messageBroker, IExecutionTimer timerStrategy) 
            : this(innerCommand, connection)
        {
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
        }

        public DbCommand InnerCommand { get; set; }

        public GlimpseDbConnection InnerConnection { get; set; } 

        public override string CommandText
        {
            get { return InnerCommand.CommandText; }
            set { InnerCommand.CommandText = value; }
        }

        public override int CommandTimeout
        {
            get { return InnerCommand.CommandTimeout; }
            set { InnerCommand.CommandTimeout = value; }
        }

        public override CommandType CommandType
        {
            get { return InnerCommand.CommandType; }
            set { InnerCommand.CommandType = value; }
        }

        public override bool DesignTimeVisible
        {
            get { return InnerCommand.DesignTimeVisible; }
            set { InnerCommand.DesignTimeVisible = value; }
        }

        public override ISite Site
        {
            get { return InnerCommand.Site; }
            set { InnerCommand.Site = value; }
        } 

        public override UpdateRowSource UpdatedRowSource
        {
            get { return InnerCommand.UpdatedRowSource; }
            set { InnerCommand.UpdatedRowSource = value; }
        }
 
        internal IMessageBroker MessageBroker
        {
            get { return messageBroker ?? (messageBroker = GlimpseRuntime.Instance.Configuration.MessageBroker); }
            set { messageBroker = value; }
        }
        
        public DbCommand Inner
        {
            get { return InnerCommand; }
        } 

        internal IExecutionTimer TimerStrategy
        {
            get { return timerStrategy ?? (timerStrategy = GlimpseRuntime.Instance.Configuration.TimerStrategy()); }
            set { timerStrategy = value; }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return InnerCommand.Parameters; }
        }

        public bool BindByName
        {
            get
            {
                var property = InnerCommand.GetType().GetProperty("BindByName");
                if (property == null)
                {
                    return false;
                }

                return (bool)property.GetValue(InnerCommand, null);
            }

            set
            {
                var property = InnerCommand.GetType().GetProperty("BindByName");
                if (property != null)
                {
                    property.SetValue(InnerCommand, value, null);
                } 
            }
        }

        public override void Cancel()
        {
            InnerCommand.Cancel();
        }

        public override void Prepare()
        {
            InnerCommand.Prepare();
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return InnerConnection;
            }

            set
            {
                InnerConnection = value as GlimpseDbConnection;
                if (InnerConnection != null)
                {
                    InnerCommand.Connection = InnerConnection.InnerConnection;
                }
                else
                { 
                    InnerConnection = new GlimpseDbConnection(value);
                    InnerCommand.Connection = InnerConnection.InnerConnection; 
                }
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return InnerCommand.Transaction == null ? null : new GlimpseDbTransaction(InnerCommand.Transaction, InnerConnection);
            }

            set
            {
                var transaction = value as GlimpseDbTransaction;
                InnerCommand.Transaction = (transaction != null) ? transaction.InnerTransaction : value;
            }
        }

        public override int ExecuteNonQuery()
        {
            int num;
            var commandId = Guid.NewGuid();

            var timer = this.LogCommandSeed();
            this.LogCommandStart(commandId, timer); 
            try
            {
                num = InnerCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                this.LogCommandError(commandId, timer, exception, "ExecuteNonQuery");
                throw;
            }

            this.LogCommandEnd(commandId, timer, num, "ExecuteNonQuery");

            return num;
        }

        public override object ExecuteScalar()
        {
            object result;
            var commandId = Guid.NewGuid();

            var timer = this.LogCommandSeed();
            this.LogCommandStart(commandId, timer);  
            try
            {
                result = InnerCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                this.LogCommandError(commandId, timer, exception, "ExecuteScalar");
                throw;
            }

            this.LogCommandEnd(commandId, timer, null, "ExecuteScalar");

            return result;
        }

        protected override DbParameter CreateDbParameter()
        {
            return InnerCommand.CreateParameter();
        }

protected override void Dispose(bool disposing)
{
    if (disposing && InnerCommand != null)
    {
        InnerCommand.Dispose();
    }

    InnerCommand = null;
    InnerConnection = null;
    base.Dispose(disposing);
}

        #if NET45
                public override async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
                {
                    EnsureConfiguration();

                    object result;
                    var commandId = Guid.NewGuid();

                    var timer = this.LogCommandSeed();
                    this.LogCommandStart(commandId, timer, true);
                    try
                    {
                        result = await InnerCommand.ExecuteScalarAsync(cancellationToken);
                    }
                    catch (Exception exception)
                    {
                        this.LogCommandError(commandId, timer, exception, "ExecuteScalarAsync", true);
                        throw;
                    }

                    this.LogCommandEnd(commandId, timer, null, "ExecuteScalarAsync", true);

                    return result;
                }

                public override async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
                {
                    EnsureConfiguration();

                    int num;
                    var commandId = Guid.NewGuid();

                    var timer = this.LogCommandSeed();
                    this.LogCommandStart(commandId, timer, true);
                    try
                    {
                        num = await InnerCommand.ExecuteNonQueryAsync(cancellationToken);
                    }
                    catch (Exception exception)
                    {
                        this.LogCommandError(commandId, timer, exception, "ExecuteNonQueryAsync", true);
                        throw;
                    }

                    this.LogCommandEnd(commandId, timer, num, "ExecuteNonQueryAsync", true);

                    return num;
                }

                protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
                {
                    EnsureConfiguration();

                    DbDataReader reader;
                    var commandId = Guid.NewGuid();

                    var timer = this.LogCommandSeed();
                    this.LogCommandStart(commandId, timer, true);
                    try
                    {
                        reader = await InnerCommand.ExecuteReaderAsync(behavior, cancellationToken);
                    }
                    catch (Exception exception)
                    {
                        this.LogCommandError(commandId, timer, exception, "ExecuteDbDataReaderAsync");
                        throw;
                    }

                    this.LogCommandEnd(commandId, timer, reader.RecordsAffected, "ExecuteDbDataReaderAsync");

                    return new GlimpseDbDataReader(reader, InnerCommand, InnerConnection.ConnectionId, commandId);
                }

                protected void EnsureConfiguration()
                {
                    if (MessageBroker == null)
                    {
                        Trace.WriteLine("GlimpseDbCommand.MessageBroker is null");
                    }

                    if (TimerStrategy == null)
                    {
                        Trace.WriteLine("GlimpseDbCommand.TimerStrategy is null");
                    }
                }
        #endif

                protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
                {
                    DbDataReader reader;
                    var commandId = Guid.NewGuid();

                    var timer = this.LogCommandSeed();
                    this.LogCommandStart(commandId, timer);
                    try
                    {
                        reader = InnerCommand.ExecuteReader(behavior);
                    }
                    catch (Exception exception)
                    {
                        this.LogCommandError(commandId, timer, exception, "ExecuteDbDataReader");
                        throw;
                    }

                    this.LogCommandEnd(commandId, timer, reader.RecordsAffected, "ExecuteDbDataReader");

                    return new GlimpseDbDataReader(reader, InnerCommand, InnerConnection.ConnectionId, commandId);
                }
    }
}
