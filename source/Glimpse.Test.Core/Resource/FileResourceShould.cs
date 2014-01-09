using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Resource;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Resource
{
    public class FileResourceShould
    {
        private readonly Mock<IRequestResponseAdapter> mockRequestResponseAdapter;
        private readonly Mock<IResourceContext> mockResourceContext;
        private readonly Mock<IResourceResultContext> mockResourceResultContext;
        private byte[] returnedContent;

        public FileResourceShould() {
            mockResourceContext = new Mock<IResourceContext>();
            mockResourceResultContext = new Mock<IResourceResultContext>();
            mockRequestResponseAdapter = new Mock<IRequestResponseAdapter>();

            mockResourceResultContext.SetupGet(c => c.RequestResponseAdapter).Returns(mockRequestResponseAdapter.Object);

            mockRequestResponseAdapter.Setup(p => p.WriteHttpResponse(It.IsAny<byte[]>())).Callback<byte[]>(content => returnedContent = content);
        }

        [Fact]
        public void BeUsableFromSeperateAssembly() {
            var tester = new FileResourceTester();

            var result = tester.Execute(mockResourceContext.Object);
            result.Execute(mockResourceResultContext.Object);

            var temp = Encoding.UTF8.GetString(returnedContent.Skip(3).ToArray());
            Assert.Equal("alert('FileResourceTester');", temp.TrimStart());
        }

        #region Nested type: FileResourceTester

        private class FileResourceTester : FileResource
        {
            public FileResourceTester() {
                Name = "FileResourceTester_js";
            }

            protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
            {
                return new EmbeddedResourceInfo(
                    this.GetType().Assembly, 
                    "Glimpse.Test.Core.Resource.FileResourceTester.js", 
                    "application/x-javascript");
            }
        }

        #endregion
    }
}