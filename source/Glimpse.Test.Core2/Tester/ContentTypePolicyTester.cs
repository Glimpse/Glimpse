using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.Policy;
using Moq;

namespace Glimpse.Test.Core2.Tester
{
    public class ContentTypePolicyTester:ContentTypePolicy
    {
        public Mock<IRuntimePolicyContext> ContextMock { get; set; }
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }

        private ContentTypePolicyTester(IList<string> contentTypes):base(contentTypes)
        {
            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.ResponseContentType).Returns(@"text/html");

            LoggerMock = new Mock<ILogger>();

            ContextMock = new Mock<IRuntimePolicyContext>();
            ContextMock.Setup(c => c.RequestMetadata).Returns(RequestMetadataMock.Object);
            ContextMock.Setup(c => c.Logger).Returns(LoggerMock.Object);
        }

        public static ContentTypePolicyTester Create()
        {
            return new ContentTypePolicyTester(new List<string>
                                       {
                                           @"text/html",
                                           @"application/json"
                                       });
        }
    }
}