using Glimpse.Core.Configuration;

namespace Glimpse.Core.Extensibility
{
    public interface IConfigurable
    {
        void Configure(GlimpseSection section);
    }
}