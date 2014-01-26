namespace Glimpse.WindowsAzure.Storage.Infrastructure
{
    public static class OperationContextFactory
    {
        static OperationContextFactory()
        {
            Current = new DefaultOperationContextFactory();
        }

        public static void SetOperationContextFactory(IOperationContextFactory factory)
        {
            lock (Current)
            {
                Current = factory;
            }
        }

        public static IOperationContextFactory Current { get; private set; }
    }
}
