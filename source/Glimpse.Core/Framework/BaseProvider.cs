namespace Glimpse.Core.Framework
{
    internal class BaseProvider
    {
        protected IReadonlyConfiguration Configuration { get; set; }

        protected ActiveGlimpseRequestContexts ActiveGlimpseRequestContexts { get; set; }

        protected IGlimpseRequestContext CurrentRequestContext
        {
            get { return ActiveGlimpseRequestContexts.Current; }
        }

        public BaseProvider(IReadonlyConfiguration configuration, ActiveGlimpseRequestContexts activeGlimpseRequestContexts)
        {
            Configuration = configuration;
            ActiveGlimpseRequestContexts = activeGlimpseRequestContexts;
        } 
    }
}