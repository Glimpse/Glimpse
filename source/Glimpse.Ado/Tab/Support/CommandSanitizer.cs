using System.Collections.Generic;
using Glimpse.Ado.Extensibility;
using Glimpse.Ado.Model;

namespace Glimpse.Ado.Tab.Support
{
    internal class CommandSanitizer
    {
        private IDictionary<string, ICommandParameterParser> Parsers { get; set; }
        private ICommandParameterParser DefaultParser { get; set; }

        public CommandSanitizer()
        {
            Parsers = new Dictionary<string, ICommandParameterParser>();
            PopuplateParsers();
        }

        public string Process(string command, IList<CommandParameterMetadata> parameters)
        {
            foreach (var parameter in parameters)
            {
                ICommandParameterParser parser;
                if (!Parsers.TryGetValue(parameter.Type, out parser))
                    parser = DefaultParser;

                command = parser.Parse(command, parameter.Name, parameter.Value, parameter.Type, parameter.Size);
            }
            return command;
        }

        private void PopuplateParsers()
        {
            var quoted = new CommandParameterParser(true);
            var unquoted = new CommandParameterParser(false);

            DefaultParser = unquoted;
            Parsers.Add("String", quoted);
        }
    }
}