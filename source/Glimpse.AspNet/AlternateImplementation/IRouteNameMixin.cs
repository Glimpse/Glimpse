namespace Glimpse.AspNet.AlternateImplementation
{
    public interface IRouteNameMixin
    {
        bool IsNamed { get; }

        string Name { get; }
    }
}