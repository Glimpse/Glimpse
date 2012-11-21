using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class ConfigurationShould
    { 
        [Fact]
        public void HaveProperContextObjectType()
        {
            var request = new Glimpse.AspNet.Tab.Configuration();

            Assert.Equal(typeof(HttpContextBase), request.RequestContextType);
        }

        [Fact]
        public void UseDefaultLifeCycleSupport()
        {
            var request = new Glimpse.AspNet.Tab.Configuration();
            Assert.Equal(RuntimeEvent.EndRequest, request.ExecuteOn);
        }

        [Fact]
        public void BeNamedConfiguration()
        {
            var request = new Glimpse.AspNet.Tab.Configuration();
            Assert.Equal("Configuration", request.Name);
        }

        [Fact]
        public void HaveADocumentationUri()
        {
            var request = new Glimpse.AspNet.Tab.Configuration();

            Assert.False(string.IsNullOrWhiteSpace(request.DocumentationUri));
        }

        [Fact]
        public void ReturnData()
        {  
            var contextMock = new Mock<ITabContext>(); 

            var request = new Glimpse.AspNet.Tab.Configuration();
            var result = request.GetData(contextMock.Object);

            //TODO: Test checks that the code runs but it doesn't check that its doing its job

            Assert.NotNull(result);
            Assert.NotNull(result as ConfigurationModel);
        } 
    }
}
