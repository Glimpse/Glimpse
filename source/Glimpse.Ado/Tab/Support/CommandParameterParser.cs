using System.Text.RegularExpressions;
using Glimpse.Ado.Extensibility;

namespace Glimpse.Ado.Tab.Support
{
    internal class CommandParameterParser : ICommandParameterParser
    {
        private const string RegexFormat = "(?<preDelimiter>[^@]){0}(?<postDelimiter>[^a-zA-Z0-9]|$)";
        private const string ReplacementFormat = "${{preDelimiter}}{0}${{postDelimiter}}";

        internal const string QuotedFormat = "'{0}' /* {1} */";
        internal const string UnquotedFormat = "{0} /* {1} */";

        public CommandParameterParser(bool useQuotes)
        { 
            UseQuotes = useQuotes;
        } 

        private bool UseQuotes { get; set; }

        public string Parse(string command, string parameterName, object parameterValue, string parameterType, int parameterSize)
        {
            if (parameterValue == null || parameterValue.ToString().Length > 50)
            {
                return command;
            }

            var formatter = UseQuotes ? QuotedFormat : UnquotedFormat;

            var regex = new Regex(string.Format(RegexFormat, Regex.Escape(parameterName)));

            return regex.Replace(command, string.Format(ReplacementFormat, string.Format(formatter, parameterValue, parameterName)));
        }
    }
}