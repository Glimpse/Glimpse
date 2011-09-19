namespace Glimpse.Core2.Extensibility
{
    public interface IServiceLocator<TRequestContext>
    {
        TRequestContext GetRequestContext();
        TModifier GetPipelineModifier<TModifier>() where TModifier:IPipelineModifier;
        IContextStore GetContextStore();
    }
}
