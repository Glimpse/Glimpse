namespace Glimpse.AspNet.AlternateImplementation
{
    public class RouteNameMixin : IRouteNameMixin
    {
        public RouteNameMixin(string name)
        {
            if (name != null)
            {
                IsNamed = true;
            }

            Name = name;
        }

        public bool IsNamed { get; private set; }

        public string Name { get; private set; }

        public static RouteNameMixin None()
        {
            return new RouteNameMixin(null);
        }
    }
}