namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// When implemented, a resource can indicate whether or not it depends on a given resource
    /// </summary>
    internal interface IDependOnResources
    {
        /// <summary>
        /// Determines whether or not the resource depends on the given resource
        /// </summary>
        /// <param name="resourceName">The internal name of the resource</param>
        /// <returns>Boolean indicating whether or not the resource depends on the given resource</returns>
        bool DependsOn(string resourceName);
    }
}