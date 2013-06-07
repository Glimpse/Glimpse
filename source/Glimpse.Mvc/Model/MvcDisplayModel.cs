namespace Glimpse.Mvc.Model
{
    public class MvcDisplayModel
    {
        public MvcDisplayModel()
        {
            ChildViewCount = -1;
        }

        public string ActionName { get; set; }

        public double? ActionExecutionTime { get; set; }

        public int ChildActionCount { get; set; }

        public string ViewName { get; set; }

        public double? ViewRenderTime { get; set; }

        public int ChildViewCount { get; set; }

        public string ControllerName { get; set; }

        public string MatchedRouteName { get; set; }
    }
}