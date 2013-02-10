using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Ado.Messages;
using Glimpse.Ado.Plumbing.Models;
using Glimpse.Ado.Tab.Support;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Ado.Tab
{
    public class SQL : TabBase, ITabSetup, IKey
    {
        public override string Name
        {
            get { return "SQL"; }
        }

        public override object GetData(ITabContext context)
        {
            var sanitizer = new CommandSanitizer();            
            var messages = context.GetMessages<AdoMessage>().ToList();
            var aggregator = new GlimpseDbQueryMessageAggregator(messages);
            var queryMetadata = aggregator.Aggregate();
               
            if (queryMetadata == null)
                return null;

            var connections = new List<object[]> { new[] { "Commands per Connection", "Open Time" } }; 
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

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<AdoMessage>();
        }
    }
}



//        private static readonly GlimpseStructuredLayout _structuredLayout = new GlimpseStructuredLayout { 
//                new GlimpseStructuredLayoutSection {
//                    new GlimpseStructuredLayoutCell { Data = 0, SuppressAutoPreview = true, 
//                        Structure = new GlimpseStructuredLayout { 
//                            new GlimpseStructuredLayoutSection {
//                                new GlimpseStructuredLayoutCell { Data = 0, Span = 6, SuppressAutoPreview = true, MinimalDisplay = true,
//                                    Structure = new GlimpseStructuredLayout { 
//                                        new GlimpseStructuredLayoutSection {
//                                            new GlimpseStructuredLayoutCell { Data = 0, Width = "150px", IsKey = true },
//                                            new GlimpseStructuredLayoutCell { Data = 1 }
//                                        }
//                                    }
//                                }
//                            },
//                            new GlimpseStructuredLayoutSection {
//                                new GlimpseStructuredLayoutCell { Data = 1, Width = "55px" },
//                                new GlimpseStructuredLayoutCell { Data = 2, IsCode = true, CodeType = "sql" },
//                                new GlimpseStructuredLayoutCell { Data = 3, Width = "25%" },
//                                new GlimpseStructuredLayoutCell { Data = 4, Width = "60px" },
//                                new GlimpseStructuredLayoutCell { Data = 5, ClassName = "mono", Postfix = " ms", Width = "100px" },
//                                new GlimpseStructuredLayoutCell { Data = 6, ClassName = "mono", Prefix = "T+ ", Postfix = " ms", Width = "70px" }
//                            },
//                            new GlimpseStructuredLayoutSection { 
//                                new GlimpseStructuredLayoutCell { Data = 8, Span = 6, SuppressAutoPreview = true, MinimalDisplay = true,
//                                    Structure = new GlimpseStructuredLayout { 
//                                        new GlimpseStructuredLayoutSection {
//                                            new GlimpseStructuredLayoutCell { Data = 0, Width = "20%" },
//                                            new GlimpseStructuredLayoutCell { Data = 1, ClassName = "mono" }
//                                        }
//                                    }
//                                }
//                            },
//                            new GlimpseStructuredLayoutSection {
//                                new GlimpseStructuredLayoutCell { Data = 7, Span = 6, SuppressAutoPreview = true, MinimalDisplay = true,
//                                    Structure = new GlimpseStructuredLayout { 
//                                        new GlimpseStructuredLayoutSection {
//                                            new GlimpseStructuredLayoutCell { Data = 0, Width = "150px", IsKey = true },
//                                            new GlimpseStructuredLayoutCell { Data = 1 }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    },
//                    new GlimpseStructuredLayoutCell { Data = 1, ClassName = "mono", Postfix = " ms", Width = "75px" }
//                }
//            };

//        public GlimpseStructuredLayout StructuredLayout
//        {
//            get { return _structuredLayout; }
//        }