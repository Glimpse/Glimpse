using System.IO;
using System.Text;
using Glimpse.AspNet;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet
{
    public class PreBodyTagFilterShould
    {
        private readonly Mock<ILogger> loggerMock;

        public PreBodyTagFilterShould()
        {
            this.loggerMock = new Mock<ILogger>();
        }

        [Fact]
        public void HaveReplacedTheClosingBodyTag()
        {
            this.DoHaveReplacedTheClosingBodyTag();
        }

        [Fact]
        public void HaveReplacedTheClosingBodyTagEvenWhenInputIsChunked()
        {
            this.DoHaveReplacedTheClosingBodyTag(10);
        }

        private void DoHaveReplacedTheClosingBodyTag(int? chunkLastNumberOfCharacters = null)
        {
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></body></html>";
            const string expectedResult = "<html><body><span>some content</span>" + htmlSnippet + "</body></html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet, "REQUEST URL", chunkLastNumberOfCharacters);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased()
        {
            this.DoHaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased();
        }

        [Fact]
        public void HaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCasedEvenWhenInputIsChunked()
        {
            this.DoHaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased(10);
        }

        public void DoHaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased(int? chunkLastNumberOfCharacters = null)
        {
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></BoDy></html>";
            const string expectedResult = "<html><body><span>some content</span>" + htmlSnippet + "</body></html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet, "REQUEST URL", chunkLastNumberOfCharacters);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTag()
        {
            this.DoHaveWrittenWarningWhenThereIsNoClosingBodyTag();
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTagEvenWhenInputIsChunked()
        {
            this.DoHaveWrittenWarningWhenThereIsNoClosingBodyTag(10);
        }

        private void DoHaveWrittenWarningWhenThereIsNoClosingBodyTag(int? chunkLastNumberOfCharacters = null)
        {
            this.loggerMock.Setup(m => m.Warn(null, (object[])null)).Verifiable();
            const string inputToProcess = "<html><body>some content</html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, "HTML SNIPPET", "REQUEST URL", chunkLastNumberOfCharacters);

            this.loggerMock.Verify(
                logger => logger.Warn(
                            "Unable to locate '</body>' with content encoding '{0}' for request '{1}'. The response may be compressed or the markup may actually be missing a '</body>' tag. See {2} for information on troubleshooting this issue.",
                            It.Is<object[]>(arguments => arguments.Length == 3 && object.Equals(arguments[0], Encoding.UTF8.EncodingName) && object.Equals(arguments[1], "REQUEST URL") && object.Equals(arguments[2], "http://getglimpse.com/Help/Troubleshooting"))),
                Times.Once());

            Assert.Equal(inputToProcess, result);
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNull()
        {
            this.DoHaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNull();
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNullEvenWhenInputIsChunked()
        {
            this.DoHaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNull(10);
        }

        private void DoHaveWrittenWarningWhenThereIsNoClosingBodyTagAndSpecifiedRequestUrlIsNull(int? chunkLastNumberOfCharacters = null)
        {
            this.loggerMock.Setup(m => m.Warn(null, (object[])null)).Verifiable();
            const string inputToProcess = "<html><body>some content</html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, "HTML SNIPPET", null, chunkLastNumberOfCharacters);

            this.loggerMock.Verify(
                logger => logger.Warn(
                            "Unable to locate '</body>' with content encoding '{0}' for request '{1}'. The response may be compressed or the markup may actually be missing a '</body>' tag. See {2} for information on troubleshooting this issue.",
                            It.Is<object[]>(arguments => arguments.Length == 3 && object.Equals(arguments[0], Encoding.UTF8.EncodingName) && object.Equals(arguments[1], "unknown") && object.Equals(arguments[2], "http://getglimpse.com/Help/Troubleshooting"))),
                Times.Once());

            Assert.Equal(inputToProcess, result);
        }

        [Fact]
        public void HaveOnlyReplacedTheLastClosingBodyTag()
        {
            this.DoHaveOnlyReplacedTheLastClosingBodyTag();
        }

        [Fact]
        public void HaveOnlyReplacedTheLastClosingBodyTagEvenWhenInputIsChunked()
        {
            this.DoHaveOnlyReplacedTheLastClosingBodyTag(20);
        }

        private void DoHaveOnlyReplacedTheLastClosingBodyTag(int? chunkLastNumberOfCharacters = null)
        {
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></body><p>some more content</p></body></html>";
            const string expectedResult = "<html><body><span>some content</span></body><p>some more content</p>" + htmlSnippet + "</body></html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet, "REQUEST URL", chunkLastNumberOfCharacters);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty()
        {
            this.DoHaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty();
        }

        [Fact]
        public void HaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmptyEvenWhenInputIsChunked()
        {
            this.DoHaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty(10);
        }

        private void DoHaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty(int? chunkLastNumberOfCharacters = null)
        {
            const string inputToProcess = "<html><body><span>some content</span></body><p>some more content</p></body></html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, null, "REQUEST URL", chunkLastNumberOfCharacters);
            Assert.Equal(inputToProcess, result);

            result = this.ProcessInputByPreBodyTagFilter(inputToProcess, string.Empty, "REQUEST URL", chunkLastNumberOfCharacters);
            Assert.Equal(inputToProcess, result);
        }

        private string ProcessInputByPreBodyTagFilter(string inputToProcess, string htmlSnippet, string requestUrl, int? chunkLastNumberOfCharacters)
        {
            using (var memoryStream = new MemoryStream())
            {
                var preBodyTagFilter = new PreBodyTagFilter(htmlSnippet, memoryStream, Encoding.UTF8, requestUrl, this.loggerMock.Object);

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