using System.Collections.Generic;
using System.Web.Routing;
using Glimpse.AspNet.AlternateImplementation;
using Glimpse.AspNet.PipelineInspector;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common; 
using Moq; 
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.PipelineInspector
{
    public class RoutesConstraintsInspectorShould
    {
        [Theory, AutoMock]
        public void WrapsRouteConstraints(RoutesConstraintInspector routesConstraintInspector, ILogger logger, IProxyFactory proxyFactory, RouteConstraint alternateConstraintImplementation, IRouteConstraint constraint, IRouteConstraint newConstraint)
        {
            var constraints = new RouteValueDictionary { { "test", constraint } };

            proxyFactory.Setup(x => x.WrapInterface(constraint, It.IsAny<IEnumerable<IAlternateMethod>>())).Returns(newConstraint).Verifiable();

            routesConstraintInspector.Setup(logger, proxyFactory, alternateConstraintImplementation, constraints);

            proxyFactory.VerifyAll();
            Assert.Same(newConstraint, constraints["test"]);
        }

        [Theory, AutoMock]
        public void WrapsRouteStrings(RoutesConstraintInspector routesConstraintInspector, ILogger logger, IProxyFactory proxyFactory, RouteConstraint alternateConstraintImplementation, string constraint, IRouteConstraint newConstraint)
        {
            var constraints = new RouteValueDictionary { { "test", constraint } };

            proxyFactory.Setup(x => x.WrapInterface(It.IsAny<IRouteConstraint>(), It.IsAny<IEnumerable<IAlternateMethod>>())).Returns(newConstraint).Verifiable();

            routesConstraintInspector.Setup(logger, proxyFactory, alternateConstraintImplementation, constraints);

            proxyFactory.VerifyAll();
            Assert.Same(newConstraint, constraints["test"]);
        }
    }
}