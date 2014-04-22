using Glimpse.Core.Configuration;

namespace Glimpse.Core.Framework
{
    internal class BaseProvider
    {
        protected IConfiguration Configuration { get; set; }

        protected ActiveGlimpseRequestContexts ActiveGlimpseRequestContexts { get; set; }

        protected IGlimpseRequestContext CurrentRequestContext
        {
            get { return ActiveGlimpseRequestContexts.Current; }
        }

        public BaseProvider(IConfiguration configuration, ActiveGlimpseRequestContexts activeGlimpseRequestContexts)
        {
            Configuration = configuration;
            ActiveGlimpseRequestContexts = activeGlimpseRequestContexts;
        } 
    }
}