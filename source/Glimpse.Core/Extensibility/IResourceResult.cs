namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for the result that is returned once resource is executed.
    /// </summary>
    public interface IResourceResult
    {
        /// <summary>
        /// Enables processing of the result of an result method by a custom type 
        /// that inherits from the <see cref="IResource"/> interface.
        /// </summary>
        /// <param name="context">
        /// The context in which the result is executed. The context information 
        /// includes the FrameworkProvider, Serializer & HtmlEncoder.
        /// </param>
        void Execute(IResourceResultContext context);
    }
}