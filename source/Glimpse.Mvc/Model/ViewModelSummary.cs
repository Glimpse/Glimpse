using System;
using System.Collections.Generic;

namespace Glimpse.Mvc.Model
{
    public class ViewModelSummary
    {
        public ViewModelSummary(IDictionary<string, object> viewData, IDictionary<string, object> tempData, Type viewDataModelType, bool modelStateIsValid, string displayModeId, Type displayModeType)
        {
            ModelType = viewDataModelType;
            IsValid = modelStateIsValid;
            TempDataKeys = tempData.Keys;
            ViewDataKeys = viewData.Keys;
            DisplayModeId = displayModeId;
            DisplayModeType = displayModeType;
        }

        public string DisplayModeId { get; set; }

        public Type DisplayModeType { get; set; }

        public Type ModelType { get; set; }
        
        public bool IsValid { get; set; }
        
        public IEnumerable<string> TempDataKeys { get; set; }
        
        public IEnumerable<string> ViewDataKeys { get; set; }

        public bool HasDisplayMode
        {
            get { return DisplayModeType != null; }
        }

        public string DisplayModeName
        {
            get
            {
                var nullType = DisplayModeType == null;
                var emptyId = string.IsNullOrEmpty(DisplayModeId);

                if (nullType)
                {
                    return null;
                }

                if (!emptyId)
                {
                    return DisplayModeId;
                }
#if MVC4
                if (DisplayModeType == typeof(System.Web.WebPages.DefaultDisplayMode) && emptyId)
                {
                    return "_Default_";
                }
#endif

                return DisplayModeType.Name;
            }
        }
    }
}