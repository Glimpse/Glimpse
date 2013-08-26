using Owin;

namespace Glimpse.Owin.Extensions
{
    public static class IAppBuilderExtensions
    {
        public static IAppBuilder WithGlimpse(this IAppBuilder app)
        {
            return new AppBuilder(app);
        }
    }
}
