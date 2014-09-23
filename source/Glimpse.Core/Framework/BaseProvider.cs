namespace Glimpse.Core.Framework
{
    internal class BaseProvider
    {
        protected IReadOnlyConfiguration Configuration { get; set; }

        protected ActiveGlimpseRequestContexts ActiveGlimpseRequestContexts { get; set; }

        protected IGlimpseRequestContext CurrentRequestContext
        {
            get { return ActiveGlimpseRequestContexts.Current; }
        }

        public BaseProvider(IReadOnlyConfiguration configuration, ActiveGlimpseRequestContexts activeGlimpseRequestContexts)
        {
            Configuration = configuration;
            ActiveGlimpseRequestContexts = activeGlimpseRequestContexts;
        } 
    }
}