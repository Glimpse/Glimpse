using System.Diagnostics.Contracts;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    [ContractClass(typeof(RuntimePolicyContextContract))]
    public interface IRuntimePolicyContext
    {
        IRequestMetadata RequestMetadata { get; }
        ILogger Logger { get; }
        T GetRequestContext<T>() where T : class;
    }

    [ContractClassFor(typeof(IRuntimePolicyContext))]
    public abstract class RuntimePolicyContextContract:IRuntimePolicyContext
    {
        public IRequestMetadata RequestMetadata
        {
            get
            {
                Contract.Ensures(Contract.Result<IRequestMetadata>()!=null);
                return default(IRequestMetadata);
            }
        }

        public ILogger Logger
        {
            get
            {
                Contract.Ensures(Contract.Result<ILogger>()!=null);
                return default(ILogger);
            }
        }

        public T GetRequestContext<T>() where T : class
        {
            return default(T);
        }
    }
}