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
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></body></html>";
            const string expectedResult = "<html><body><span>some content</span>" + htmlSnippet + "</body></html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveReplacedTheClosingBodyTagEvenWhenBodyTagIsBadlyCased()
        {
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></BoDy></html>";
            const string expectedResult = "<html><body><span>some content</span>" + htmlSnippet + "</body></html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveWrittenWarningWhenThereIsNoClosingBodyTag()
        {
            this.loggerMock.Setup(m => m.Warn(null, (object[])null)).Verifiable();
            const string inputToProcess = "<html><body>some content</html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, "HTML SNIPPET");

            this.loggerMock.Verify(
                logger => logger.Warn(
                            "Unable to locate '</body>' with content encoding '{0}'. Response may be compressed.",
                            It.Is<object[]>(arguments => arguments.Length == 1 && object.Equals(arguments[0], Encoding.UTF8.EncodingName))),
                Times.Once());

            Assert.Equal(inputToProcess, result);
        }

        [Fact]
        public void HaveOnlyReplacedTheLastClosingBodyTag()
        {
            const string htmlSnippet = "MY HTML SNIPPET";
            const string inputToProcess = "<html><body><span>some content</span></body><p>some more content</p></body></html>";
            const string expectedResult = "<html><body><span>some content</span></body><p>some more content</p>" + htmlSnippet + "</body></html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, htmlSnippet);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HaveReplacedTheLastClosingBodyTagWithOnlyAnotherClosingBodyTagWhenTheHtmlSnippetIsNullOrEmpty()
        {
            const string inputToProcess = "<html><body><span>some content</span></body><p>some more content</p></body></html>";
            string result = this.ProcessInputByPreBodyTagFilter(inputToProcess, null);
            Assert.Equal(inputToProcess, result);

            result = this.ProcessInputByPreBodyTagFilter(inputToProcess, string.Empty);
            Assert.Equal(inputToProcess, result);
        }

        private string ProcessInputByPreBodyTagFilter(string inputToProcess, string htmlSnippet)
        {
            using (var memoryStream = new MemoryStream())
            {
                var preBodyTagFilter = new PreBodyTagFilter(htmlSnippet, memoryStream, Encoding.UTF8, this.loggerMock.Object);

                byte[] buffer = Encoding.UTF8.GetBytes(inputToProcess);
                preBodyTagFilter.Write(buffer, 0, buffer.Length);
                preBodyTagFilter.Flush();
                preBodyTagFilter.Position = 0;

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}