using Glimpse.NH.Plumbing.Profiler;

namespace Glimpse.NH.Plumbing.Injectors
{
    public interface INHibernateDriverInfo
    {
        bool IsWrapped();
        object GetDriver();
        void SetDriver(IGlimpseProfileDbDriver driver);
    }
}