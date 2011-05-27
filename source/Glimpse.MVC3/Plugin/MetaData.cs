using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = false)]
    internal class MetaData : IGlimpsePlugin, IProvideGlimpseHelp
    {
        public string Name
        {
            get { return "MetaData"; }
        }

        public object GetData(HttpApplication application)
        {
            var store = application.Context.Items;
            var data = store[GlimpseConstants.ViewEngine] as IList<GlimpseViewEngineCallMetadata>;

            if (data == null) 
                return null;

            var result = new List<object[]>
                             {
                                 new[]
                                     {
                                         "Registration", "Type", "Details"
                                     }
                             };


            foreach (var callMetadata in data)
            {
                if (callMetadata.ViewEngineResult.View != null)
                {
                    var viewContext = callMetadata.GlimpseView.ViewContext;
                    var metaData = viewContext.ViewData.ModelMetadata;

                    object detailResult = null;
                    object propertyResults = null;
                    string meatDataType = null;

                    if (metaData != null)
                    {
                        detailResult = ProcessMetaData(metaData);
                        meatDataType = metaData.ModelType.ToString();

                        if (metaData.Properties != null && metaData.Properties.Count() > 0)
                        {
                            var propertyResult = new Dictionary<string, object>();
                            foreach (var metaDataProperty in metaData.Properties)
                                propertyResult.Add(metaDataProperty.PropertyName, ProcessMetaData(metaDataProperty));
                            propertyResults = propertyResult;
                        }
                    }
                    else
                        detailResult = "No MetaData found.";

                    result.Add(new[] { "Primary View Model", meatDataType, detailResult, propertyResults == null ? "glimpse-start-open" : "" });
                    if (propertyResults != null)
                        result.Add(new[] { "View Model Properties", null, propertyResults });


                    break;
                }
            }

            return result;
        }

        protected object ProcessMetaData(ModelMetadata metaData)
        {
            return new
                        {
                            ConvertEmptyStringToNull = ParseValue(metaData.ConvertEmptyStringToNull, true), //A flag which indicates whether empty strings that are posted back in forms should be converted into NULLs. Default: true
                            DataTypeName = ParseValue(metaData.DataTypeName, null), //A string which can used to give meta information about the data type (for example, to let you know that this string is actually an e-mail address). Some well-known data type names include “EmailAddress”, “Html”, “Password”, and “Url”. Default: null
                            Description = ParseValue(metaData.Description, null), //A long-form textual description of this model. Default: null
                            DisplayFormatString = ParseValue(metaData.DisplayFormatString, null), //A format string that will be used when displaying this model value in a template. Default: null
                            DisplayName = ParseValue(metaData.DisplayName, null), //The display name of this model value. Used in templates and Html.Label/LabelFor to generate the label text. Default: null
                            EditFormatString = ParseValue(metaData.EditFormatString, null), //A format string that will be used when editing this model value in a template. Default: null
                            HideSurroundingHtml = ParseValue(metaData.HideSurroundingHtml, false), //A flag which indicates that this field should not have any of its surrounding HTML (for example, a label). Often used when a template will be generating a hidden input. Default: null
                            metaData.IsComplexType, //A flag which indicates whether the system considers this to be a complex type (and therefore will default to the complex object template rather than the string template). Not user-settable
                            metaData.IsNullableValueType, //A flag which indicates whether the model is a nullable value type (namely, Nullable<T>). Not user-settable
                            IsReadOnly = ParseValue(metaData.IsReadOnly, false), //A flag which indicates if this value is read-only (for example, because the property does not have a setter). Default: false
                            IsRequired = ParseValue(metaData.IsRequired, metaData.IsNullableValueType), //A flag which indicates if this value is required. Default: true for non-nullable value types; false for all others.
                            NullDisplayText = ParseValue(metaData.NullDisplayText, null), //The text which should be used when attempting to display a null model. Default: null
                            ShortDisplayName = ParseValue(metaData.ShortDisplayName, null), //The short display name of this mode value. Intended to be used in the title of tabular list views. If this field is null, then DisplayName should be used. Default: null
                            ShowForDisplay = ParseValue(metaData.ShowForDisplay, true), //A flag which indicates if this model should be shown in display mode. Default: true
                            ShowForEdit = ParseValue(metaData.ShowForEdit, true), //A flag which indicates if this model should be shown in edit mode. Default: true
                            metaData.SimpleDisplayText, //Text which should be shown for this model when summarizing what would otherwise be a complex object display. Default: see below
                            TemplateHint = ParseValue(metaData.TemplateHint, null), //A string which indicates a hint as to what template should be used for this model. Default: null
                            Watermark = ParseValue(metaData.Watermark, null), //Text that might be displayed as a watermark when editing this model in a text box. Default: null
                        };
        }

        protected object ParseValue(object currentValue, object defaultValue)
        {
            if (currentValue == null)
                return currentValue;

            var cur = currentValue.ToString();
            var def = defaultValue == null ? "" : defaultValue.ToString();

            if (cur != def)
                return "*" + cur + "*";
            return cur;
        }

        public void SetupInit(HttpApplication application)
        {
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/MetaData"; }
        }
    }
}