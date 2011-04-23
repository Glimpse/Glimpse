using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Extensibility;
using Glimpse.Net.Extensions;
using Glimpse.Net.Plumbing;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin(ShouldSetupInInit = false)]
    public class MetaData : IGlimpsePlugin
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

                    object detailResult = new
                                              {
                                                  metaData.ConvertEmptyStringToNull, //A flag which indicates whether empty strings that are posted back in forms should be converted into NULLs. Default: true
                                                  metaData.DataTypeName, //A string which can used to give meta information about the data type (for example, to let you know that this string is actually an e-mail address). Some well-known data type names include “EmailAddress”, “Html”, “Password”, and “Url”. Default: null
                                                  metaData.Description, //A long-form textual description of this model. Default: null
                                                  metaData.DisplayFormatString, //A format string that will be used when displaying this model value in a template. Default: null
                                                  metaData.DisplayName, //The display name of this model value. Used in templates and Html.Label/LabelFor to generate the label text. Default: null
                                                  metaData.EditFormatString, //A format string that will be used when editing this model value in a template. Default: null
                                                  metaData.HideSurroundingHtml, //A flag which indicates that this field should not have any of its surrounding HTML (for example, a label). Often used when a template will be generating a hidden input. Default: null
                                                  metaData.IsComplexType, //A flag which indicates whether the system considers this to be a complex type (and therefore will default to the complex object template rather than the string template). Not user-settable
                                                  metaData.IsNullableValueType, //A flag which indicates whether the model is a nullable value type (namely, Nullable<T>). Not user-settable
                                                  metaData.IsReadOnly, //A flag which indicates if this value is read-only (for example, because the property does not have a setter). Default: false
                                                  metaData.IsRequired, //A flag which indicates if this value is required. Default: true for non-nullable value types; false for all others.
                                                  metaData.NullDisplayText, //The text which should be used when attempting to display a null model. Default: null
                                                  metaData.ShortDisplayName, //The short display name of this mode value. Intended to be used in the title of tabular list views. If this field is null, then DisplayName should be used. Default: null
                                                  metaData.ShowForDisplay, //A flag which indicates if this model should be shown in display mode. Default: true
                                                  metaData.ShowForEdit, //A flag which indicates if this model should be shown in edit mode. Default: true
                                                  metaData.SimpleDisplayText, //Text which should be shown for this model when summarizing what would otherwise be a complex object display. Default: see below
                                                  metaData.TemplateHint, //A string which indicates a hint as to what template should be used for this model. Default: null
                                                  metaData.Watermark, //Text that might be displayed as a watermark when editing this model in a text box. Default: null
                                              };



                    result.Add(new[]
                                   {
                                       "Primary View Model", metaData.ModelType.ToString(), detailResult
                                   });

                    break;
                }
            }

            return result;
        }

        public void SetupInit(HttpApplication application)
        {
            throw new NotImplementedException();
        }
    }
}