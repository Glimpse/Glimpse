using System.IO;
using System.Text;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core
{
    public class GlimpseScriptsInjectionStreamShould
    {
        private Mock<ILogger> LoggerMock { get; set; }

        public GlimpseScriptsInjectionStreamShould()
        {
            LoggerMock = new Mock<ILogger>();
        }

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
            string result = ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet, "REQUEST URL", chunkLastNumberOfCharacters);
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
            string result = ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet, "REQUEST URL", chunkLastNumberOfCharacters);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTag()
        {
            DoHaveWrittenWarningWhenThereIsNoClosingBodyTag();
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTagEvenWhenInputIsChunked()
        {
            DoHaveWrittenWarningWhenThereIsNoClosingBodyTag(10);
        }

        private void DoHaveWrittenWarningWhenThereIsNoClosingBodyTag(int? chunkLastNumberOfCharacters = null)
        {
            LoggerMock.Setup(m => m.Warn(null, (object[])null)).Verifiable();
            const string inputToProcess = "<html><body>some content</html>";
            string result = ProcessInputByPreBodyTagFilter(inputToProcess, "HTML SNIPPET", "REQUEST URL", chunkLastNumberOfCharacters);

            LoggerMock.Verify(
                logger => logger.Warn(
                            "Unable to locate '</body>' with content encoding '{0}' for request '{1}'. The response may be compressed or the markup may actually be missing a '</body>' tag. See {2} for information on troubleshooting this issue.",
                            It.Is<object[]>(arguments => arguments.Length == 3 && object.Equals(arguments[0], Encoding.UTF8.EncodingName) && object.Equals(arguments[1], "REQUEST URL") && object.Equals(arguments[2], "http://getglimpse.com/Help/Troubleshooting"))),
                Times.Once());

            Assert.Equal(inputToProcess, result);
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNull()
        {
            DoHaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNull();
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNullEvenWhenInputIsChunked()
        {
            DoHaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNull(10);
        }

        private void DoHaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNull(int? chunkLastNumberOfCharacters = null)
        {
            LoggerMock.Setup(m => m.Warn(null, (object[])null)).Verifiable();
            const string inputToProcess = "<html><body>some content</html>";
            string result = ProcessInputByPreBodyTagFilter(inputToProcess, "HTML SNIPPET", null, chunkLastNumberOfCharacters);

            LoggerMock.Verify(
                logger => logger.Warn(
                            "Unable to locate '</body>' with content encoding '{0}' for request '{1}'. The response may be compressed or the markup may actually be missing a '</body>' tag. See {2} for information on troubleshooting this issue.",
                            It.Is<object[]>(arguments => arguments.Length == 3 && object.Equals(arguments[0], Encoding.UTF8.EncodingName) && object.Equals(arguments[1], "unknown") && object.Equals(arguments[2], "http://getglimpse.com/Help/Troubleshooting"))),
                Times.Once());

            Assert.Equal(inputToProcess, result);
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
            string result = ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet, "REQUEST URL", chunkLastNumberOfCharacters);
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
            string result = ProcessInputByPreBodyTagFilter(inputToProcess, null, "REQUEST URL", chunkLastNumberOfCharacters);
            Assert.Equal(inputToProcess, result);

            result = ProcessInputByPreBodyTagFilter(inputToProcess, string.Empty, "REQUEST URL", chunkLastNumberOfCharacters);
            Assert.Equal(inputToProcess, result);
        }

        private string ProcessInputByPreBodyTagFilter(string inputToProcess, string htmlSnippet, string requestUrl, int? chunkLastNumberOfCharacters)
        {
            using (var memoryStream = new MemoryStream())
            {
                var preBodyTagFilter = new GlimpseScriptsInjectionStream(() => htmlSnippet, memoryStream, () => Encoding.UTF8, () => requestUrl, LoggerMock.Object);

                string[] inputsToProcess = { inputToProcess };
                if (chunkLastNumberOfCharacters.HasValue)
                {
                    inputsToProcess = new[] { inputToProcess.Substring(0, inputToProcess.Length - chunkLastNumberOfCharacters.Value), inputToProcess.Substring(inputToProcess.Length - chunkLastNumberOfCharacters.Value) };
                }

                foreach (string inputToProcessChunk in inputsToProcess)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(inputToProcessChunk);
                    preBodyTagFilter.Write(buffer, 0, buffer.Length);
                }

                preBodyTagFilter.Flush();
                preBodyTagFilter.Position = 0;

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}