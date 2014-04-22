using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common; 
using System.Reflection;
using Glimpse.Ado.Message;
using Glimpse.Core.Message;

namespace Glimpse.Ado.AlternateType
{
    public static class Support
    {
        public static DbProviderFactory TryGetProviderFactory(this DbConnection connection)
        {
            // If we can pull it out quickly and easily
            var profiledConnection = connection as GlimpseDbConnection;
            if (profiledConnection != null)
            {
                return profiledConnection.InnerProviderFactory;
            }

#if (NET45)
            return DbProviderFactories.GetFactory(connection);
#else
            return connection.GetType().GetProperty("ProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(connection, null) as DbProviderFactory;
#endif
        }

        public static DbProviderFactory TryGetProfiledProviderFactory(this DbConnection connection)
        {
            var factory = connection.TryGetProviderFactory();
            if (factory != null)
            { 
                if (!(factory is GlimpseDbProviderFactory))
                {
                    factory = factory.WrapProviderFactory(); 
                }
            }
            else
            {
                throw new NotSupportedException(string.Format(Resources.DbFactoryNotFoundInDbConnection, connection.GetType().FullName));
            }

            return factory;
        }

        public static DbProviderFactory WrapProviderFactory(this DbProviderFactory factory)
        {
            if (!(factory is GlimpseDbProviderFactory))
            { 
                var factoryType = typeof(GlimpseDbProviderFactory<>).MakeGenericType(factory.GetType());
                return (DbProviderFactory)factoryType.GetField("Instance").GetValue(null);    
            }

            return factory;
        }

        public static DataTable FindDbProviderFactoryTable()
        {
            var providerFactories = typeof(DbProviderFactories);
            var providerField = providerFactories.GetField("_configTable", BindingFlags.NonPublic | BindingFlags.Static) ?? providerFactories.GetField("_providerTable", BindingFlags.NonPublic | BindingFlags.Static);
            var registrations = providerField.GetValue(null);
            return registrations is DataSet ? ((DataSet)registrations).Tables["DbProviderFactories"] : (DataTable)registrations;
        }

        public static object GetParameterValue(IDataParameter parameter)
        {
            if (parameter.Value == DBNull.Value)
            {
                return "NULL";
            }

            if (parameter.Value is byte[])
            {
                var blob = parameter.Value as byte[];
                return "BLOB" + (blob != null ? string.Format(" {0} bytes", blob.Length) : string.Empty);
            }

            return parameter.Value;
        }

        public static TimeSpan LogCommandSeed(this GlimpseDbCommand command)
        {
            return command.TimerStrategy != null ? command.TimerStrategy.Start() : TimeSpan.Zero;
        }

        public static void LogCommandStart(this GlimpseDbCommand command, Guid commandId, TimeSpan timerTimeSpan)
        {
            command.LogCommandStart(commandId, timerTimeSpan, false);
        }

        public static void LogCommandStart(this GlimpseDbCommand command, Guid commandId, TimeSpan timerTimeSpan, bool isAsync)
        {
            if (command.MessageBroker != null)
            {
                IList<CommandExecutedParamater> parameters = null;
                if (command.Parameters.Count > 0)
                {
                    parameters = new List<CommandExecutedParamater>();
                    foreach (IDbDataParameter parameter in command.Parameters)
                    {
                        var parameterName = parameter.ParameterName;
                        if (!parameterName.StartsWith("@"))
                        {
                            parameterName = "@" + parameterName;
                        }

                        parameters.Add(new CommandExecutedParamater { Name = parameterName, Value = GetParameterValue(parameter), Type = parameter.DbType.ToString(), Size = parameter.Size });
                    }
                }

                command.MessageBroker.Publish(
                    new CommandExecutedMessage(command.InnerConnection.ConnectionId, commandId, command.InnerCommand.CommandText, parameters, command.InnerCommand.Transaction != null, isAsync)
                    .AsTimedMessage(timerTimeSpan));
            }
        }

        public static void LogCommandEnd(this GlimpseDbCommand command, Guid commandId, TimeSpan timer, int? recordsAffected, string type)
        {
            command.LogCommandEnd(commandId, timer, recordsAffected, type, false);
        }

        public static void LogCommandEnd(this GlimpseDbCommand command, Guid commandId, TimeSpan timer, int? recordsAffected, string type, bool isAsync)
        {
            if (command.MessageBroker != null && command.TimerStrategy != null)
            {
                command.MessageBroker.Publish(
                    new CommandDurationAndRowCountMessage(command.InnerConnection.ConnectionId, commandId, recordsAffected)
                    .AsTimedMessage(command.TimerStrategy.Stop(timer))
                    .AsTimelineMessage("Command: Executed", AdoTimelineCategory.Command, type));
            }
        }

        public static void LogCommandError(this GlimpseDbCommand command, Guid commandId, TimeSpan timer, Exception exception, string type)
        {
            command.LogCommandError(commandId, timer, exception, type, false);
        }

        public static void LogCommandError(this GlimpseDbCommand command, Guid commandId, TimeSpan timer, Exception exception, string type, bool isAsync)
        {
            if (command.MessageBroker != null && command.TimerStrategy != null)
            {
                command.MessageBroker.Publish(
                    new CommandErrorMessage(command.InnerConnection.ConnectionId, commandId, exception)
                    .AsTimedMessage(command.TimerStrategy.Stop(timer))
                    .AsTimelineMessage("Command: Error", AdoTimelineCategory.Command, type));
            }
        }
    }
}
