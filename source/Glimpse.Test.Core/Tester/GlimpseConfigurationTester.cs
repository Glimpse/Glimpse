using System;
using System.Diagnostics;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core;
using Glimpse.Test.Core.Extensions;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class GlimpseConfigurationTester : GlimpseConfiguration
    {
        private GlimpseConfigurationTester(Mock<IFrameworkProvider> frameworkProviderMock,
                                           Mock<ILogger> loggerMock,
                                           IDiscoverableCollection<IResource> resourcesStub,
                                           IDiscoverableCollection<ITab> tabsStub,
                                           IDiscoverableCollection<IRuntimePolicy> policiesStub)
            : base(
                frameworkProviderMock.Object,
                loggerMock.Object,
                resourcesStub,
                tabsStub,
                policiesStub)
        {
            FrameworkProviderMock = frameworkProviderMock;
            LoggerMock = loggerMock;
        }

        public static GlimpseConfigurationTester Create()
        {
            var loggerMock = new Mock<ILogger>();

            return new GlimpseConfigurationTester(new Mock<IFrameworkProvider>().Setup(),
                                                  loggerMock,
                                                  new ReflectionDiscoverableCollection<IResource>(loggerMock.Object),
                                                  new ReflectionDiscoverableCollection<ITab>(loggerMock.Object),
                                                  new ReflectionDiscoverableCollection<IRuntimePolicy>(loggerMock.Object));
        }

        public Mock<IFrameworkProvider> FrameworkProviderMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }
    }
}