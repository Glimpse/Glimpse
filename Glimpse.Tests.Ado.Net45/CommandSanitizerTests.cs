using System.Collections.Generic;
using System.Text;
using Glimpse.Ado.Model;
using Glimpse.Ado.Tab.Support;
using Xunit;

namespace Glimpse.Tests.Ado.Net45
{
    public class CommandSanitizerTests
    {
        /// <summary>
        /// In this case @q1 should not replace @q10
        /// </summary>
        [Fact]
        public void ShouldNotReplaceParameterNameWhenItIsSubsetOfOtherParameterName()
        {
            var commandSanitizer = new CommandSanitizer();
            var parameters = new List<CommandParameterMetadata>();
            var expectedParameters = new StringBuilder();
            var commandParameters = new StringBuilder();

            for (var i = 0; i < 11; i += 1)
            {
                var parameterName = string.Format("@q{0}", i);

                parameters.Add(new CommandParameterMetadata
                {
                    Name = parameterName,
                    Size = 0,
                    Type = "Int32",
                    Value = i
                });

                commandParameters.AppendFormat("{0},", parameterName);
                expectedParameters.AppendFormat("{0} /* {1} */,", i, parameterName);
            }

            const string command = "I'm a command ";
            var commandWithParameters = (command + commandParameters).TrimEnd(',');

            var actualOutput = commandSanitizer.Process(commandWithParameters, parameters);

            var expectedOutput = (command + expectedParameters).TrimEnd(',');
            Assert.Equal(expectedOutput, actualOutput);
        }
    }
}
