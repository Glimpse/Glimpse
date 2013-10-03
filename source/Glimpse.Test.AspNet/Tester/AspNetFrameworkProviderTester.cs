using System.Collections.Generic;
using System.Web;
using Glimpse.AspNet;
using Glimpse.Core.Extensibility;
using Moq;

namespace Glimpse.Test.AspNet.Tester
{
    public class AspNetFrameworkProviderTester : AspNetFrameworkProvider
    {
        public Mock<HttpContextBase> HttpContextMock { get; set; }

        public Mock<HttpApplicationStateBase> HttpApplicationStateMock { get; set; }

        public Mock<HttpResponseBase> HttpResponseMock { get; set; }
    
        private AspNetFrameworkProviderTester(HttpContextBase context, ILogger logger) : base(context, logger)
        {
            HttpResponseMock = new Mock<HttpResponseBase>();

            HttpApplicationStateMock = new Mock<HttpApplicationStateBase>();
            HttpApplicationStateMock.Setup(st => st.Get("testKey")).Returns("testValue");

            HttpContextMock = new Mock<HttpContextBase>();
            HttpContextMock.Setup(ctx => ctx.Response).Returns(HttpResponseMock.Object);
            HttpContextMock.Setup(c => c.Application).Returns(HttpApplicationStateMock.Object);
            HttpContextMock.Setup(ctx => ctx.Items)
                .Returns(new Dictionary<object, object>
                             {
                                 { typeof(string).AssemblyQualifiedName, "TestString" },
                                 { typeof(int).AssemblyQualifiedName, 5 }
                             });

            Context = HttpContextMock.Object;
        }

        public static AspNetFrameworkProviderTester Create()
        {
            return new AspNetFrameworkProviderTester(new Mock<HttpContextBase>().Object, new Mock<ILogger>().Object); // TODO: Fix Hack
        }
    }
}