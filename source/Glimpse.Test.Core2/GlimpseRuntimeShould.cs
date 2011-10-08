using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseRuntimeShould
    {
        public GlimpseConfiguration Configuration { get; set; }

        public GlimpseRuntimeShould()
        {
            var runtimeServiceMock = new Mock<IRuntimeService>();
            var runtimeService = runtimeServiceMock.Object;

            Configuration = new GlimpseConfiguration(runtimeService);
        }

        [Fact]
        public void ConstructWithConfiguration()
        {
            var runtime = new GlimpseRuntime(Configuration);

            Assert.NotNull(runtime);
        }

        [Fact]
        public void BeginRequest()
        {
            var runtime = new GlimpseRuntime(Configuration);

            //start a stopwatch
            //init storage space
            //create request id

            //TODO:Comeback to me

            //runtime.BeginRequest();

            //Assert.NotNull(runtime.ServiceLocator);
        }
    }
}
