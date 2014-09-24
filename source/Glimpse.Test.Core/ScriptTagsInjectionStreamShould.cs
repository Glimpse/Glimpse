using System;
using System.IO;
using System.Text;
using Glimpse.Core.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core
{
    public class ScriptTagsInjectionStreamShould
    {
        [Fact]
        public void HaveReplacedTheClosingBodyTag()
        {
            DoHaveReplacedTheClosingBodyTag();
        }

        [Fact]
        public void HaveReplacedTheClosingBodyTagEvenWhenInputIsChunked()
        {
            DoHaveReplacedTheClosingBodyTag(10);
        }

        private void DoHaveReplacedTheClosingBodyTag(int? chunkLastNumberOfCharacters = null)
        {
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></body></html>";
            const string expectedResult = "<html><body><span>some content</span>" + htmlSnippet + "</body></html>";
            string result = ProcessInputByScriptTagsInjectionStream(inputToProcess, htmlSnippet, chunkLastNumberOfCharacters);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased()
        {
            DoHaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased();
        }

        [Fact]
        public void HaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCasedEvenWhenInputIsChunked()
        {
            DoHaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased(10);
        }

        public void DoHaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased(int? chunkLastNumberOfCharacters = null)
        {
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></BoDy></html>";
            const string expectedResult = "<html><body><span>some content</span>" + htmlSnippet + "</body></html>";
            string result = ProcessInputByScriptTagsInjectionStream(inputToProcess, htmlSnippet, chunkLastNumberOfCharacters);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void NotifyWhenInjectionFailedDueToMisingClosingBodyTag()
        {
            DoNotifyWhenInjectionFailedDueToMisingClosingBodyTag();
        }

        [Fact]
        public void NotifyWhenInjectionFailedDueToMisingClosingBodyTagEvenWhenInputIsChunked()
        {
            DoNotifyWhenInjectionFailedDueToMisingClosingBodyTag(10);
        }

        private void DoNotifyWhenInjectionFailedDueToMisingClosingBodyTag(int? chunkLastNumberOfCharacters = null)
        {
            const string inputToProcess = "<html><body>some content</html>";
            string failureMessage = null;

            string result = ProcessInputByScriptTagsInjectionStream(inputToProcess, "HTML SNIPPET", chunkLastNumberOfCharacters, (sender, args) => failureMessage = args.FailureMessage);

            Assert.Equal(inputToProcess, result);
            Assert.Equal(
                failureMessage,
                "Unable to locate '</body>' with content encoding '" + Encoding.UTF8.EncodingName + "'. The response may be compressed or the markup may actually be missing a '</body>' tag. See http://getglimpse.com/Help/Troubleshooting for information on troubleshooting this issue.");
        }

        [Fact]
        public void HaveOnlyReplacedTheLastClosingBodyTag()
        {
            DoHaveOnlyReplacedTheLastClosingBodyTag();
        }

        [Fact]
        public void HaveOnlyReplacedTheLastClosingBodyTagEvenWhenInputIsChunked()
        {
            DoHaveOnlyReplacedTheLastClosingBodyTag(20);
        }

        private void DoHaveOnlyReplacedTheLastClosingBodyTag(int? chunkLastNumberOfCharacters = null)
        {
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></body><p>some more content</p></body></html>";
            const string expectedResult = "<html><body><span>some content</span></body><p>some more content</p>" + htmlSnippet + "</body></html>";
            string result = ProcessInputByScriptTagsInjectionStream(inputToProcess, htmlSnippet, chunkLastNumberOfCharacters);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty()
        {
            DoHaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty();
        }

        [Fact]
        public void HaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmptyEvenWhenInputIsChunked()
        {
            DoHaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty(10);
        }

        private void DoHaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty(int? chunkLastNumberOfCharacters = null)
        {
            const string inputToProcess = "<html><body><span>some content</span></body><p>some more content</p></body></html>";
            string result = ProcessInputByScriptTagsInjectionStream(inputToProcess, null, chunkLastNumberOfCharacters);
            Assert.Equal(inputToProcess, result);

            result = ProcessInputByScriptTagsInjectionStream(inputToProcess, string.Empty, chunkLastNumberOfCharacters);
            Assert.Equal(inputToProcess, result);
        }

        private static string ProcessInputByScriptTagsInjectionStream(string inputToProcess, string htmlSnippet, int? chunkLastNumberOfCharacters, EventHandler<ScriptTagsInjectionFailedEventArgs> onInjectionFailed = null)
        {
            using (var memoryStream = new MemoryStream())
            {
                var scriptTagsProviderMock = new Mock<IScriptTagsProvider>();
                scriptTagsProviderMock.Setup(scriptTagsProvider => scriptTagsProvider.ScriptTagsAllowedToBeProvided).Returns(true);
                scriptTagsProviderMock.Setup(scriptTagsProvider => scriptTagsProvider.GetScriptTags()).Returns(htmlSnippet);
                var options = new ScriptTagsInjectionOptions(scriptTagsProviderMock.Object, () => Encoding.UTF8, onInjectionFailed);

                using (var scriptTagsInjectionStream = new ScriptTagsInjectionStream(memoryStream, options))
                {
                    string[] inputsToProcess = { inputToProcess };
                    if (chunkLastNumberOfCharacters.HasValue)
                    {
                        inputsToProcess = new[] { inputToProcess.Substring(0, inputToProcess.Length - chunkLastNumberOfCharacters.Value), inputToProcess.Substring(inputToProcess.Length - chunkLastNumberOfCharacters.Value) };
                    }

                    foreach (string inputToProcessChunk in inputsToProcess)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(inputToProcessChunk);
                        scriptTagsInjectionStream.Write(buffer, 0, buffer.Length);
                    }

                    scriptTagsInjectionStream.Flush();
                    scriptTagsInjectionStream.Position = 0;
                }

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}