using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    internal interface IPrivilegedResource:IResource
    {
        IResourceResult Execute(IResourceContext context, IGlimpseConfiguration configuration); 
    }
}