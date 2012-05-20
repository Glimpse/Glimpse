using Glimpse.Core2.Configuration;

namespace Glimpse.Core2.Extensibility
{
    public interface IConfigurable
    {
        void Configure(GlimpseSection section);
    }
}