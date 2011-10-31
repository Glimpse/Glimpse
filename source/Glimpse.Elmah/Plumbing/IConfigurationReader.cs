namespace Glimpse.Elmah.Plumbing
{
	public interface IConfigurationReader
	{
	    string GetPathFor<THandler>();
	}
}