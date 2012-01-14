using System.Collections.Generic;
using System.Text.RegularExpressions;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.Policy;
using Moq;

namespace Glimpse.Test.Core2.Tester
{
    public class UriPolicyTester : UriPolicy
    {
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }
        public Mock<IRuntimePolicyContext> ContextMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }

        private UriPolicyTester():base(new List<Regex>{new Regex("blocked")})
        {
            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.RequestUri).Returns("shouldn't matter!");

            LoggerMock = new Mock<ILogger>();

            ContextMock = new Mock<IRuntimePolicyContext>();
            ContextMock.Setup(c => c.RequestMetadata).Returns(RequestMetadataMock.Object);
            ContextMock.Setup(c => c.Logger).Returns(LoggerMock.Object);
        }

        public static UriPolicyTester Create()
        {
            return new UriPolicyTester();
        }
    }
}