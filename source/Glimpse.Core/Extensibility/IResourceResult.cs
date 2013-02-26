namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IResourceResult</c> provides Glimpse a <see href="http://en.wikipedia.org/wiki/Command_pattern">command object</see> for returning the results of an <see cref="IResource"/>.
    /// </summary>
    public interface IResourceResult
    {
        /// <summary>
        /// Executes the resource result with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        void Execute(IResourceResultContext context);
    }
}