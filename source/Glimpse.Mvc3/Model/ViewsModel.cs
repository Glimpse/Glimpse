using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;

namespace Glimpse.Mvc.Model
{
    public class ViewsModel
    {
        public ViewsModel(ViewEngine.FindViews.Message viewEngineFindView, View.Render.Message viewRender)
        {
            ViewName = viewEngineFindView.Input.ViewName;
            MasterName = viewEngineFindView.Input.MasterName;
            IsPartial = viewEngineFindView.IsPartial;
            ViewEngineType = viewEngineFindView.BaseType;
            UseCache = viewEngineFindView.Input.UseCache;
            IsFound = viewEngineFindView.IsFound;
            SearchedLocations = viewEngineFindView.Output.SearchedLocations;

            if (viewRender != null)
            {
                var viewContext = viewRender.Input.ViewContext;
                ViewModelSummary = new ViewModelSummary(viewContext.ViewData, viewContext.TempData);
            }
        }

        public string ViewName { get; set; }
        public string MasterName { get; set; }
        public bool IsPartial { get; set; }
        public Type ViewEngineType { get; set; }
        public bool UseCache { get; set; }
        public bool IsFound { get; set; }
        public IEnumerable<string> SearchedLocations { get; set; }
        public ViewModelSummary ViewModelSummary { get; set; }
    }

    public class ViewModelSummary
    {
        public ViewModelSummary(ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            if (viewData.Model != null)
                ModelType = viewData.Model.GetType();

            IsValid = viewData.ModelState.IsValid;
            TempDataKeys = tempData.Keys;
            ViewDataKeys = viewData.Keys;
        }

        public Type ModelType { get; set; }
        public bool IsValid { get; set; }
        public IEnumerable<string> TempDataKeys { get; set; }
        public IEnumerable<string> ViewDataKeys { get; set; }
    }
}