using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Glimpse.Mvc.Model
{
    public class ViewModelSummary
    {
        public ViewModelSummary(IDictionary<string, object> viewData, IDictionary<string, object> tempData, Type viewDataModelType, bool modelStateIsValid)
        {
            ModelType = viewDataModelType;
            IsValid = modelStateIsValid;
            TempDataKeys = tempData.Keys;
            ViewDataKeys = viewData.Keys;
        }

        public Type ModelType { get; set; }
        
        public bool IsValid { get; set; }
        
        public IEnumerable<string> TempDataKeys { get; set; }
        
        public IEnumerable<string> ViewDataKeys { get; set; }
    }
}