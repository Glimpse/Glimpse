namespace Glimpse.WebApi.Core.Model
{
    public class RouteConstraintModel
    {
        public string ParameterName { get; set; }
         
        public string Constraint { get; set; }
          
        public bool? IsMatch { get; set; }
    }
}