using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Ado.Plugin.Support;
using Glimpse.Ado.Plumbing.Models;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Ado.Tab
{
    public class Sql : TabBase<HttpContextBase>, IKey
    {
        public override string Name
        {
            get { return "Sql"; }
        }

        public override object GetData(ITabContext context)
        {
            var sanitizer = new CommandSanitizer();
            var foo = context.GetMessages<GlimpseDbQueryMetadata>();
            var queryMetadata = context.GetRequestContext<HttpContextBase>().Items["Glimpse.Db.Query"] as GlimpseDbQueryMetadata;            

            if(queryMetadata == null)
            {
                return null;
            }

            var connections = new List<object[]> { new object[] { "Commands per Connection", "Open Time" } };
            foreach (var connection in queryMetadata.Connections.Values)
            {
                if (connection.Commands.Count == 0 && connection.Transactions.Count == 0)
                    continue;

                var commands = new List<object[]> { new[] { "Transaction Start", "Ordinal", "Command", "Parameters", "Records", "Command Time", "From First", "Transaction End", "Errors" } };
                var commandCount = 1;
                foreach (var command in connection.Commands.Values)
                {
                    //Transaction Start
                    List<object[]> headTransaction = null;
                    if (command.HeadTransaction != null)
                        headTransaction = new List<object[]> { new[] { "Name", "Isolation Level" }, new[] { "Transaction Started", command.HeadTransaction.IsolationLevel } };

                    //Transaction Finish
                    List<object[]> tailTransaction = null;
                    if (command.TailTransaction != null)
                        tailTransaction = new List<object[]> { new[] { "Name", "Committed" }, new[] { "Transaction Complete", command.TailTransaction.Committed ? "Committed" : "Rollbacked" } };

                    //Parameters
                    List<object[]> parameters = null;
                    if (command.Parameters.Count > 0)
                    {
                        parameters = new List<object[]> { new[] { "Name", "Value", "Type", "Size" } };
                        foreach (var parameter in command.Parameters)
                            parameters.Add(new[] { parameter.Name, parameter.Value, parameter.Type, parameter.Size });
                    }

                    //Exception
                    List<object[]> errors = null;
                    if (command.Exception != null)
                    {
                        var exception = command.Exception.GetBaseException();
                        var exceptionName = command.Exception != exception ? command.Exception.Message + ": " + exception.Message : exception.Message;

                        errors = new List<object[]> { new[] { "Error", "Stack" }, new[] { exceptionName, exception.StackTrace } };
                    }

                    //Commands
                    var records = command.RecordsAffected == null || command.RecordsAffected < 0 ? command.TotalRecords : command.RecordsAffected;
                    commands.Add(new object[] { headTransaction, commandCount++, sanitizer.Process(command.Command, command.Parameters), parameters, records, command.ElapsedMilliseconds, "0", tailTransaction, errors, errors != null ? "error" : "" });
                }
                var elapse = 0.0;
                //TODO: Can we use a stopwatch here?
                if (connection.EndDateTime.HasValue && connection.StartDateTime.HasValue)
                    elapse = Convert.ToInt32(connection.EndDateTime.Value.Subtract(connection.StartDateTime.Value).TotalMilliseconds);
                connections.Add(new object[] { commands, elapse });
            }

            return connections.Count > 1 ? connections : null;   
        }

        public string Key
        {
            get { return "glimpse_sql"; }
        }        
    }
}