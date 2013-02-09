using System.Collections.Generic;
using Glimpse.Ado.Extensibility;
using Glimpse.Ado.Plumbing.Models;

namespace Glimpse.Ado.Plugin.Support
{
    internal class CommandSanitizer
    {
        private IDictionary<string, IGlimpseCommandParameterParser> Parsers { get; set; }
        private IGlimpseCommandParameterParser DefaultParser { get; set; }

        public CommandSanitizer()
        {
            Parsers = new Dictionary<string, IGlimpseCommandParameterParser>();
            PopuplateParsers();
        }

        public string Process(string command, IList<GlimpseDbQueryCommandParameterMetadata> parameters)
        {
            foreach (var parameter in parameters)
            {
                IGlimpseCommandParameterParser parser;
                if (!Parsers.TryGetValue(parameter.Type, out parser))
                    parser = DefaultParser;

                command = parser.Parse(command, parameter.Name, parameter.Value, parameter.Type, parameter.Size);
            }
            return command;
        }

        private void PopuplateParsers()
        {
            var quoted = new GlimpseCommandParameterParser(true);
            var unquoted = new GlimpseCommandParameterParser(false);

            DefaultParser = unquoted;
            Parsers.Add("String", quoted);
        }
    }
}