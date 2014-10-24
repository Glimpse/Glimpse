using Owin;

namespace Glimpse.Owin
{
    public static class IAppBuilderExtensions
    {
        public static IAppBuilder WithGlimpse(this IAppBuilder app)
        {
            return new AppBuilder(app);
        }
    }
}
