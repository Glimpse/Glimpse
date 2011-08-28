using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.EF.Plugin.Support;
using Glimpse.EF.Plumbing;
using Glimpse.EF.Plumbing.Models;

namespace Glimpse.EF.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    internal class EF : IGlimpsePlugin, IProvideGlimpseHelp, IProvideGlimpseStructuredLayout
    {
        private CommandSanitizer Sanitizer { get; set; }
        private IGlimpseLogger Logger { get; set; }
        internal const string StoreKey = "Glimpse.DbQuery";
        
        
        [ImportingConstructor]
        public EF(IGlimpseFactory factory)
        {
            Logger = factory.CreateLogger();
            Sanitizer = new CommandSanitizer();
        }

        public string Name
        {
            get { return "SQL"; }
        }

        public object GetData(HttpContextBase context)
        {
            var contextStore = context.Items;
            var queryMetadata = contextStore[StoreKey] as GlimpseDbQueryMetadata;
            if (queryMetadata == null)
                return null;

            var connections = new List<object[]> { new[] { "Commands per Connection", "Open Time" } };
            foreach (var connection in queryMetadata.Connections.Values)
            {
                var commands = new List<object[]> { new[] { "Ordinal", "Command", "Parameters", "Records", "Command Time", "From First", "Errors" } };
                var commandCount = 1;
                foreach (var command in connection.Commands.Values)
                {
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

                        errors = new List<object[]> { new[] { "Error", "Stack" }, new[] { exceptionName, exception.StackTrace }}; 
                    }

                    //Commands
                    var records = command.RecordsAffected == null || command.RecordsAffected < 0 ? command.TotalRecords : command.RecordsAffected;
                    commands.Add(new object[] { commandCount++, Sanitizer.Process(command.Command, command.Parameters), parameters, records, command.ElapsedMilliseconds, "0", errors, errors != null ? "error" : "" }); 
                }
                var elapse = 0.0;
                //TODO: Can we use a stopwatch here?
                if (connection.EndDateTime.HasValue && connection.StartDateTime.HasValue)
                    elapse = Convert.ToInt32(connection.EndDateTime.Value.Subtract(connection.StartDateTime.Value).TotalMilliseconds);
                connections.Add(new object[] { commands, elapse });
            }

            return connections;
        }

        public void SetupInit()
        {
            try
            {
                var initiator = new AdoPipelineInitiator(Logger);
                initiator.Initiate();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/EF"; }
        }

        #region IProvideGlimpseStructuredLayout
        //TODO: Move to client prerender plugin

        private static readonly GlimpseStructuredLayout _structuredLayout = new GlimpseStructuredLayout { 
                new GlimpseStructuredLayoutSection {
                    new GlimpseStructuredLayoutCell { Data = 0, SuppressAutoPreview = true, 
                        Structure = new GlimpseStructuredLayout { 
                            new GlimpseStructuredLayoutSection {
                                new GlimpseStructuredLayoutCell { Data = 0, Width = "55px" },
                                new GlimpseStructuredLayoutCell { Data = 1, IsCode = true, CodeType = "sql" },
                                new GlimpseStructuredLayoutCell { Data = 2, Width = "25%" },
                                new GlimpseStructuredLayoutCell { Data = 3, Width = "60px" },
                                new GlimpseStructuredLayoutCell { Data = 4, ClassName = "mono", Postfix = " ms", Width = "100px" },
                                new GlimpseStructuredLayoutCell { Data = 5, ClassName = "mono", Prefix = "T+ ", Postfix = " ms", Width = "70px" }
                            },
                            new GlimpseStructuredLayoutSection { 
                                new GlimpseStructuredLayoutCell { Data = 6, Span = 6, SuppressAutoPreview = true, MinimalDisplay = true,
                                    Structure = new GlimpseStructuredLayout { 
                                        new GlimpseStructuredLayoutSection {
                                            new GlimpseStructuredLayoutCell { Data = 0, Width = "20%" },
                                            new GlimpseStructuredLayoutCell { Data = 1, ClassName = "mono" }
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
    }
}
