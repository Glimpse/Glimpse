using System.Collections.Generic;
using Glimpse.EF.Extensibility; 
using Glimpse.EF.Plumbing.Models;

namespace Glimpse.EF.Plugin.Support
{
    internal class CommandSanitizer
    {
        private IDictionary<string, ICommandParamaterParser> Parsers { get; set; }
        private ICommandParamaterParser DefaultParser { get; set; }

        public CommandSanitizer()
        {
            Parsers = new Dictionary<string, ICommandParamaterParser>();
            PopuplateParsers();
        }

        public string Process(string command, IList<GlimpseDbQueryCommandParameterMetadata> parameters)
        {
            foreach (var parameter in parameters)
            {
                ICommandParamaterParser parser;
                if (!Parsers.TryGetValue(parameter.Type, out parser))
                    parser = DefaultParser;
                command = parser.Parse(command, parameter.Name, parameter.Value, parameter.Type, parameter.Size);
            }
            return command;
        }

        private void PopuplateParsers()
        {
            var quoted = new GlimpseCommandParamaterParser(true);
            var unquoted = new GlimpseCommandParamaterParser(false);

            DefaultParser = unquoted;
            Parsers.Add("String", quoted);
        }
    }
}