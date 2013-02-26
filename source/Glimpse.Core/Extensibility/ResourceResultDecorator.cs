namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An <see cref="IResourceResult"/> abstraction for adding additional functionality to <see cref="IResourceResult"/> via the common <see href="http://en.wikipedia.org/wiki/Decorator_pattern">decorator pattern</see>.
    /// </summary>
    public abstract class ResourceResultDecorator : IResourceResult, IWrapper<IResourceResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceResultDecorator" /> class.
        /// </summary>
        /// <param name="wrappedResourceResult">The wrapped resource result.</param>
        protected ResourceResultDecorator(IResourceResult wrappedResourceResult)
        {
            WrappedResourceResult = wrappedResourceResult;
        }

        /// <summary>
        /// Gets or sets the wrapped resource result.
        /// </summary>
        /// <value>
        /// The wrapped resource result.
        /// </value>
        protected IResourceResult WrappedResourceResult { get; set; }

        /// <summary>
        /// Gets the wrapped object.
        /// </summary>
        /// <returns>
        /// Returned the wrapped object.
        /// </returns>
        public IResourceResult GetWrappedObject()
        {
            return WrappedResourceResult;
        }

        /// <summary>
        /// Executes the resource result with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(IResourceResultContext context)
        {
            Decorate(context);
            WrappedResourceResult.Execute(context);
        }

        /// <summary>
        /// Decorates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected abstract void Decorate(IResourceResultContext context);
    }
}