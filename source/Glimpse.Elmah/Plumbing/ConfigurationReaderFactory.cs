using System.Web;

namespace Glimpse.Elmah.Plumbing
{
	public class ConfigurationReaderFactory : IConfigurationReaderFactory
	{
		public IConfigurationReader Create()
		{
			if (HttpRuntime.UsingIntegratedPipeline)
			{
				return new ConfigurationReaderForIntegratedPipelineMode();
			}

			return new ConfigurationReaderForClassicMode();
		}
	}
}