using System;
using System.Globalization;
using System.Linq;
using Glimpse.Ado.Model;
using Glimpse.Ado.Tab.Support;
using Xunit;

namespace Glimpse.Test.Ado
{
    public class CommandSanitizerShould
    {
        [Fact]
        public void ReplaceParameterPlaceholders()
        {
            var sut = new CommandSanitizer();

            var parameter = new CommandParameterMetadata
            {
                Name = "@SomeParameter",
                Type = typeof(int).Name,
                Value = 1234
            };

            var command = string.Format("SELECT * FROM Table WHERE Id = {0}", parameter.Name);

            var result = sut.Process(command, new[] { parameter });

            var expected = string.Format(CommandParameterParser.UnquotedFormat, parameter.Value, parameter.Name);

            Assert.Contains(expected, result);
        }

        [Fact]
        public void WrapStringsInQuotes()
        {
            var sut = new CommandSanitizer();

            var parameter = new CommandParameterMetadata
            {
                Name = "@StringParameter",
                Type = typeof(string).Name,
                Value = "This is the parameter value."
            };

            var command = string.Format("SELECT * FROM Table WHERE StringColumn = {0}", parameter.Name);

            var result = sut.Process(command, new[] { parameter });

            var expected = string.Format(CommandParameterParser.QuotedFormat, parameter.Value, parameter.Name);

            Assert.Contains(expected, result);
        }

        [Fact]
        public void NotReplacePartialMatches()
        {
            var sut = new CommandSanitizer();

            var parameters = Enumerable.Range(1, 30).Select(x => new CommandParameterMetadata
            {
                Name = "@Id" + x,
                Type = typeof(int).Name,
                Value = x
            }).ToList();

            var parameterList = string.Join(", ", parameters.Select(x => x.Name));

            var command = string.Format("SELECT * FROM Table WHERE Id IN ({0})", parameterList);

            var result = sut.Process(command, parameters);

            foreach (var parameter in parameters)
            {
                var expected = string.Format(CommandParameterParser.UnquotedFormat, parameter.Value, parameter.Name);

                Assert.Contains(expected, result);
            }
        }
    }
}