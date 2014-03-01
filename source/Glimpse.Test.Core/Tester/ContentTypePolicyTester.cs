using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Policy;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class ContentTypePolicyTester:ContentTypePolicy
    {
        public Mock<IRuntimePolicyContext> ContextMock { get; set; }
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }

        private ContentTypePolicyTester(IList<Tuple<string, RuntimePolicy>> contentTypes)
            : base(contentTypes)
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
            return new ContentTypePolicyTester(new List<Tuple<string, RuntimePolicy>>
                                       {
                                           new Tuple<string, RuntimePolicy>(@"text/html", RuntimePolicy.On),
                                           new Tuple<string, RuntimePolicy>(@"application/json", RuntimePolicy.PersistResults)
                                       });
        }
    }
}