namespace Glimpse.Core2
{
    public class GlimpseRuntime
    {
        private GlimpseConfiguration Configuration { get; set; }

        public GlimpseRuntime(GlimpseConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void UpdateConfiguration(GlimpseConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
