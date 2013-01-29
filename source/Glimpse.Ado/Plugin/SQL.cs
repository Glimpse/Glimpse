using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Ado.Plugin.Support;
using Glimpse.Ado.Plumbing.Models;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.Ado.Plugin
{
    public class SQL : TabBase<HttpContextBase>, IDocumentation, ITabLayout
    {
        public const string StoreKey = "Glimpse.DbQuery"; 
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).DisablePreview().SetLayout(TabLayout.Create().Row(x =>
                            x.Cell(0).SpanColumns(6).DisablePreview().AsMinimalDisplay().SetLayout(TabLayout.Create().Row(y =>  
                            {
                                y.Cell(0).WidthInPixels(150).AsKey();
                                y.Cell(1);
                            }))).Row(x =>
                            {
                                x.Cell(1).WidthInPixels(55);
                                x.Cell(2).AsCode(CodeType.Sql).DisablePreview();
                                x.Cell(3).WidthInPercent(25).DisablePreview();
                                x.Cell(4).WidthInPixels(60);
                                x.Cell(5).WidthInPixels(100).Suffix(" ms").Class("mono");
                                x.Cell(6).WidthInPixels(70).Prefix("T+ ").Suffix(" ms").Class("mono");
                            }).Row(x =>
                            x.Cell(8).SpanColumns(6).DisablePreview().AsMinimalDisplay().SetLayout(TabLayout.Create().Row(y =>  
                            {
                                y.Cell(0).WidthInPercent(20);
                                y.Cell(1).Class("mono").DisablePreview();
                            }))).Row(x =>
                            x.Cell(7).SpanColumns(6).DisablePreview().AsMinimalDisplay().SetLayout(TabLayout.Create().Row(y => 
                            {
                                y.Cell(0).WidthInPixels(150).AsKey();
                                y.Cell(1);
                            }))));
                    r.Cell(1).WidthInPixels(75).Suffix(" ms").Class("mono");
                }).Build();

        public SQL()
        {
            Sanitizer = new CommandSanitizer();
        }

        public override string Name
        {
            get { return "SQL"; }
        }

        public string DocumentationUri
        {
            get { return "http://getGlimpse.com/Help/Plugin/SQL"; }
        }

        private CommandSanitizer Sanitizer { get; set; }

        public object GetLayout()
        {
            return Layout;
        }

        public override object GetData(ITabContext context)
        {
            var contextStore = context.GetRequestContext<HttpContextBase>().Items;
            var queryMetadata = contextStore[StoreKey] as GlimpseDbQueryMetadata;
            if (queryMetadata == null)
            {
                return null;
            }

            var connections = new List<object[]> { new[] { "Commands per Connection", "Open Time" } };
            foreach (var connection in queryMetadata.Connections.Values)
            {
                if (connection.Commands.Count == 0 && connection.Transactions.Count == 0)
                {
                    continue;
                }

                var commands = new List<object[]> { new string[] { "Transaction Start", "Ordinal", "Command", "Parameters", "Records", "Command Time", "From First", "Transaction End", "Errors" } };
                var commandCount = 1;
                foreach (var command in connection.Commands.Values)
                {
                    // Transaction Start
                    List<object[]> headTransaction = null;
                    if (command.HeadTransaction != null)
                    {
                        headTransaction = new List<object[]>
                            {
                                new[] { "Name", "Isolation Level" },
                                new[] { "Transaction Started", command.HeadTransaction.IsolationLevel }
                            };
                    }

                    // Transaction Finish
                    List<object[]> tailTransaction = null;
                    if (command.TailTransaction != null)
                    {
                        tailTransaction = new List<object[]>
                            {
                                new[] { "Name", "Committed" },
                                new[] { "Transaction Complete", command.TailTransaction.Committed ? "Committed" : "Rollbacked" }
                            };
                    }

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
                    commands.Add(new object[] { headTransaction, commandCount++, Sanitizer.Process(command.Command, command.Parameters), parameters, records, command.ElapsedMilliseconds, "0", tailTransaction, errors, errors != null ? "error" : "" });
                }

                var elapse = TimeSpan.Zero;
                ////TODO: Can we use a stopwatch here?
                if (connection.EndDateTime.HasValue && connection.StartDateTime.HasValue)
                {
                    elapse = connection.EndDateTime.Value.Subtract(connection.StartDateTime.Value);
                }

                connections.Add(new object[] { commands, elapse });
            }

            return connections.Count > 1 ? connections : null; 
        }
         
         /* TODO: Convert this over to the new interface and syntax
        #region IProvideGlimpseStructuredLayout
        //TODO: Move to client prerender plugin

        private static readonly GlimpseStructuredLayout _structuredLayout = new GlimpseStructuredLayout { 
                new GlimpseStructuredLayoutSection {
                    new GlimpseStructuredLayoutCell { Data = 0, ForceFull = true, 
                        Structure = new GlimpseStructuredLayout { 
                            new GlimpseStructuredLayoutSection {
                                new GlimpseStructuredLayoutCell { Data = 0, Span = 6, ForceFull = true, MinimalDisplay = true,
                                    Structure = new GlimpseStructuredLayout { 
                                        new GlimpseStructuredLayoutSection {
                                            new GlimpseStructuredLayoutCell { Data = 0, Width = "150px", IsKey = true },
                                            new GlimpseStructuredLayoutCell { Data = 1 }
                                        }
                                    }
                                }
                            },
                            new GlimpseStructuredLayoutSection {
                                new GlimpseStructuredLayoutCell { Data = 1, Width = "55px" },
                                new GlimpseStructuredLayoutCell { Data = 2, IsCode = true, CodeType = "sql" },
                                new GlimpseStructuredLayoutCell { Data = 3, Width = "25%" },
                                new GlimpseStructuredLayoutCell { Data = 4, Width = "60px" },
                                new GlimpseStructuredLayoutCell { Data = 5, ClassName = "mono", Postfix = " ms", Width = "100px" },
                                new GlimpseStructuredLayoutCell { Data = 6, ClassName = "mono", Prefix = "T+ ", Postfix = " ms", Width = "70px" }
                            },
                            new GlimpseStructuredLayoutSection { 
                                new GlimpseStructuredLayoutCell { Data = 8, Span = 6, ForceFull = true, MinimalDisplay = true,
                                    Structure = new GlimpseStructuredLayout { 
                                        new GlimpseStructuredLayoutSection {
                                            new GlimpseStructuredLayoutCell { Data = 0, Width = "20%" },
                                            new GlimpseStructuredLayoutCell { Data = 1, ClassName = "mono" }
                                        }
                                    }
                                }
                            },
                            new GlimpseStructuredLayoutSection {
                                new GlimpseStructuredLayoutCell { Data = 7, Span = 6, ForceFull = true, MinimalDisplay = true,
                                    Structure = new GlimpseStructuredLayout { 
                                        new GlimpseStructuredLayoutSection {
                                            new GlimpseStructuredLayoutCell { Data = 0, Width = "150px", IsKey = true },
                                            new GlimpseStructuredLayoutCell { Data = 1 }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new GlimpseStructuredLayoutCell { Data = 1, ClassName = "mono", Postfix = " ms", Width = "75px" }
                }
            };

        public GlimpseStructuredLayout StructuredLayout
        {
            get { return _structuredLayout; }
        }

        #endregion 
        */
    }
}
