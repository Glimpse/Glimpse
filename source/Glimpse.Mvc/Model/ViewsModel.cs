using System;
using System.Collections.Generic;
using Glimpse.Mvc.AlternateType;

namespace Glimpse.Mvc.Model
{
    public class ViewsModel
    {
        public ViewsModel(ViewEngine.FindViews.Message viewEngineFindView, View.Render.Message viewRender)
        { 
            ViewName = viewEngineFindView.ViewName;
            MasterName = viewEngineFindView.MasterName;
            IsPartial = viewEngineFindView.IsPartial;
            ViewEngineType = viewEngineFindView.BaseType;
            UseCache = viewEngineFindView.UseCache;
            IsFound = viewEngineFindView.IsFound;
            SearchedLocations = viewEngineFindView.SearchedLocations;

            if (viewRender != null)
            {
                SourceController = viewRender.ControllerName;
                ViewModelSummary = new ViewModelSummary(viewRender.ViewData, viewRender.TempData, viewRender.ViewDataModelType, viewRender.ModelStateIsValid, viewEngineFindView.DisplayModeId, viewEngineFindView.DisplayModeType);
            }
        }

        public object SourceController { get; set; }

        public string ViewName { get; set; }

        public string MasterName { get; set; }
        
        public bool IsPartial { get; set; }
        
        public Type ViewEngineType { get; set; }
        
        public bool UseCache { get; set; }
        
        public bool IsFound { get; set; }
        
        public IEnumerable<string> SearchedLocations { get; set; }
        
        public ViewModelSummary ViewModelSummary { get; set; }
    }
}