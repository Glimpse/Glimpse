namespace Glimpse.AspNet.Model
{
    public class RouteConstraintModel
    {
        public string ParameterName { get; set; }
         
        public string Constraint { get; set; }
         
        public bool Checked { get; set; }
         
        public bool Matched { get; set; }
    }
}