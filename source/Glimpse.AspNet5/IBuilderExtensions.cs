using Microsoft.AspNet.Builder;

namespace Glimpse.AspNet5
{
    public static class IBuilderExtensions
    {
        public static IApplicationBuilder WithGlimpse(this IApplicationBuilder app)
        {
            return new Builder(app);
        }
    }
}
