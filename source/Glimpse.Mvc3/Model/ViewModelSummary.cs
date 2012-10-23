using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Glimpse.Mvc.Model
{
    public class ViewModelSummary
    {
        public ViewModelSummary(ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            if (viewData.Model != null)
            {
                ModelType = viewData.Model.GetType();
            }

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