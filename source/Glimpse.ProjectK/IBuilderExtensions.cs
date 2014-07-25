using Microsoft.AspNet.Builder;

namespace Glimpse.ProjectK
{
    public static class IBuilderExtensions
    {
        public static IBuilder WithGlimpse(this IBuilder app)
        {
            return new Builder(app);
        }
    }
}
