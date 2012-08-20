using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    internal interface IPrivilegedResource:IResource
    {
        IResourceResult Execute(IResourceContext context, IGlimpseConfiguration configuration); 
    }
}