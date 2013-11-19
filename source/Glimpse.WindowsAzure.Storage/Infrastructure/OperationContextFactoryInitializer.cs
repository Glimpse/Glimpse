using Glimpse.Core.Extensibility;

namespace Glimpse.WindowsAzure.Storage.Infrastructure
{
    public class OperationContextFactoryInitializer
        : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            OperationContextFactory.SetOperationContextFactory(new GlimpseOperationContextFactory(context.MessageBroker, context.TimerStrategy));
        }
    }
}