using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.Tab
{
    public class Views : AspNetTab, IDocumentation, ITabSetup, ITabLayout, IKey
    {
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(60);
                    r.Cell(1).WidthInPixels(160);
                    r.Cell(2).WidthInPixels(160);
                    r.Cell(3);
                    r.Cell(4).WidthInPixels(60);
                    r.Cell(5).WidthInPercent(15);
                    r.Cell(6).WidthInPixels(80);
                    r.Cell(7).WidthInPixels(60);
                    r.Cell(8);
                }).Build();

        public override string Name
        {
            get { return "Views"; }
        }

        public string Key
        {
            get { return "glimpse_views"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Views-Tab"; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public override object GetData(ITabContext context)
        {
            var viewEngineFindViewsMessages = context.GetMessages<ViewEngine.FindViews.Message>();
            var viewRenderMessages = context.GetMessages<View.Render.Message>();
            var result = new List<ViewsModel>();

            if (viewEngineFindViewsMessages == null || viewRenderMessages == null)
            {
                return result;
            }

            foreach (var findView in viewEngineFindViewsMessages)
            {
                result.Add(new ViewsModel(findView, viewRenderMessages.SingleOrDefault(r => r.ViewCorrelation.ViewEngineFindCallId == findView.Id)));
            }

            return result;
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ViewEngine.FindViews.Message>();
            context.PersistMessages<View.Render.Message>();
        }
    }
}