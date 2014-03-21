namespace Glimpse.WebApi.AlternateType
{
    public interface IRouteNameMixin
    {
        bool IsNamed { get; }

        string Name { get; }
    }
}