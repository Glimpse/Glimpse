using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text;
#if NET45
using System.Threading;
using System.Threading.Tasks;
#endif
using Glimpse.Ado.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Ado.AlternateType
{
    public class GlimpseDbCommand : DbCommand
    {
        private IMessageBroker messageBroker;

        public GlimpseDbCommand(DbCommand innerCommand)
        {
            InnerCommand = innerCommand; 
        }

        public GlimpseDbCommand(DbCommand innerCommand, GlimpseDbConnection connection) :
            this(innerCommand)
        {
            InnerConnection = connection;
        }
         
        public GlimpseDbCommand(DbCommand innerCommand, GlimpseDbConnection connection, IMessageBroker messageBroker) :
            this(innerCommand, connection)
        {
            MessageBroker = messageBroker;
        }

        private DbCommand InnerCommand { get; set; }

        private GlimpseDbConnection InnerConnection { get; set; } 
 
        private IMessageBroker MessageBroker
        {
            get { return messageBroker ?? (messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker()); }
            set { messageBroker = value; }
        }

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

        protected override DbParameterCollection DbParameterCollection
        {
            get { return InnerCommand.Parameters; }
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

        public override void Cancel()
        {
            InnerCommand.Cancel();
        }

        public override void Prepare()
        {
            InnerCommand.Prepare();
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

        protected override DbConnection DbConnection
        {
            get { return InnerConnection; }
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

        protected override DbParameter CreateDbParameter()
        {
            return InnerCommand.CreateParameter();
        }
        
        public DbCommand Inner
        {
            get { return InnerCommand; }
        } 

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            DbDataReader reader;
            var commandId = Guid.NewGuid();

            LogCommandStart(commandId); 
            var stopwatch = Stopwatch.StartNew();
            try
            {
                reader = InnerCommand.ExecuteReader(behavior);
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                LogCommandError(commandId, stopwatch.Elapsed, exception);
                throw;
            }

            stopwatch.Stop();
            LogCommandEnd(commandId, stopwatch.Elapsed, reader.RecordsAffected);

            return new GlimpseDbDataReader(reader, InnerCommand, InnerConnection.ConnectionId, commandId); 
        }

        public override int ExecuteNonQuery()
        {
            int num;
            var commandId = Guid.NewGuid();

            LogCommandStart(commandId); 
            var stopwatch = Stopwatch.StartNew();
            try
            {
                num = InnerCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                LogCommandError(commandId, stopwatch.Elapsed, exception);
                throw;
            }
            stopwatch.Stop();
            LogCommandEnd(commandId, stopwatch.Elapsed, num);

            return num;
        }

        public override object ExecuteScalar()
        {
            object result;
            var commandId = Guid.NewGuid();

            LogCommandStart(commandId); 
            var stopwatch = Stopwatch.StartNew();
            try
            {
                result = InnerCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                LogCommandError(commandId, stopwatch.Elapsed, exception);
                throw;
            }
            LogCommandEnd(commandId, stopwatch.Elapsed, null);

            return result;
        }

#if NET45
        protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            return InnerCommand.ExecuteReaderAsync(behavior, cancellationToken);
        }


        public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            return InnerCommand.ExecuteScalarAsync(cancellationToken);
        }


        public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            return InnerCommand.ExecuteNonQueryAsync(cancellationToken);
        }
#endif

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
         
        #region Support Methods
        private static object GetParameterValue(IDataParameter parameter)
        {
            if (parameter.Value == DBNull.Value)
            {
                return "NULL";
            } 

            if (parameter.Value is byte[])
            {
                var builder = new StringBuilder("0x");
                foreach (var num in (byte[])parameter.Value)
                {
                    builder.Append(num.ToString("X2"));
                } 

                return builder.ToString();
            }
            return parameter.Value;
        }

        private void LogCommandStart(Guid commandId)
        {
            if (MessageBroker != null)
            {
                IList<CommandExecutedParamater> parameters = null;
                if (Parameters.Count > 0)
                {
                    parameters = new List<CommandExecutedParamater>();
                    foreach (IDbDataParameter parameter in Parameters)
                    {
                        var parameterName = parameter.ParameterName;
                        if (!parameterName.StartsWith("@"))
                        {
                            parameterName = "@" + parameterName;
                        }

                        parameters.Add(new CommandExecutedParamater { Name = parameterName, Value = GetParameterValue(parameter), Type = parameter.DbType.ToString(), Size = parameter.Size });
                    }
                }

                MessageBroker.Publish(new CommandExecutedMessage(InnerConnection.ConnectionId, commandId, InnerCommand.CommandText, parameters));
            }
        }

        private void LogCommandEnd(Guid commandId, TimeSpan elapsed, int? recordsAffected)
        {
            if (MessageBroker != null)
            {
                MessageBroker.Publish(new CommandDurationAndRowCountMessage(InnerConnection.ConnectionId, commandId, elapsed, recordsAffected));
            } 
        }

        private void LogCommandError(Guid commandId, TimeSpan elapsed, Exception exception)
        {
            if (MessageBroker != null)
            {
                MessageBroker.Publish(new CommandErrorMessage(InnerConnection.ConnectionId, commandId, elapsed, exception));
            }
        }
        #endregion
    }
}
