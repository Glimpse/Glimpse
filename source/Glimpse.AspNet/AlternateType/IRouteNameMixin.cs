namespace Glimpse.AspNet.AlternateType
{
    public interface IRouteNameMixin
    {
        bool IsNamed { get; }

        string Name { get; }
    }
}