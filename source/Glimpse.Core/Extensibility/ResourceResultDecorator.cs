namespace Glimpse.Core.Extensibility
{
    public abstract class ResourceResultDecorator : IResourceResult, IWrapper<IResourceResult>
    {
        protected ResourceResultDecorator(IResourceResult wrappedResourceResult)
        {
            WrappedResourceResult = wrappedResourceResult;
        }

        protected IResourceResult WrappedResourceResult { get; set; }

        public IResourceResult GetWrappedObject()
        {
            return WrappedResourceResult;
        }

        public void Execute(IResourceResultContext context)
        {
            Decorate(context);
            WrappedResourceResult.Execute(context);
        }

        protected abstract void Decorate(IResourceResultContext context);
    }
}