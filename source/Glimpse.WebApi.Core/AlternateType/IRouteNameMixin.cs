namespace Glimpse.WebApi.Core.AlternateType
{
    public interface IRouteNameMixin
    {
        bool IsNamed { get; }

        string Name { get; }
    }
}