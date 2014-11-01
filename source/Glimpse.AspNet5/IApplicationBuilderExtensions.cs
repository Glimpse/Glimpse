using Microsoft.AspNet.Builder;

namespace Glimpse.AspNet5
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder WithGlimpse(this IApplicationBuilder app)
        {
            return new ApplicationBuilder(app);
        }
    }
}
