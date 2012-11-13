using System.Diagnostics.CodeAnalysis;

namespace Glimpse.Mvc.Model
{
    public class MetadataContentModel
    {
        public MetadataContentModel()
        {
            ConvertEmptyStringToNull = new DefaultValueModel<bool> { Default = true };
            DataTypeName = new DefaultValueModel<string>();
            Description = new DefaultValueModel<string>();
            DisplayFormatString = new DefaultValueModel<string>();
            DisplayName = new DefaultValueModel<string>();
            EditFormatString = new DefaultValueModel<string>();
            HideSurroundingHtml = new DefaultValueModel<bool>();
            IsComplexType = new DefaultValueModel<bool>();
            IsNullableValueType = new DefaultValueModel<bool>();
            IsReadOnly = new DefaultValueModel<bool>();
            IsRequired = new DefaultValueModel<bool>();
            NullDisplayText = new DefaultValueModel<string>();
            ShortDisplayName = new DefaultValueModel<string>();
            ShowForDisplay = new DefaultValueModel<bool> { Default = true };
            ShowForEdit = new DefaultValueModel<bool> { Default = true };
            SimpleDisplayText = new DefaultValueModel<string>();
            TemplateHint = new DefaultValueModel<string>();
            Watermark = new DefaultValueModel<object>();
            Order = new DefaultValueModel<int>();
        }

        public DefaultValueModel<int> Order { get; set; }

        /// <summary>
        /// Gets a flag which indicates whether empty strings that are posted back in forms should be converted into NULLs. Default: true
        /// </summary>
        public DefaultValueModel<bool> ConvertEmptyStringToNull { get; private set; }
        
        /// <summary>
        /// Gets a string which can used to give meta information about the data type (for example, to let you know that this string is actually an e-mail address). Some well-known data type names include “EmailAddress”, “Html”, “Password”, and “Url”. Default: null
        /// </summary>
        public DefaultValueModel<string> DataTypeName { get; private set; }
        
        /// <summary>
        /// Gets a long-form textual description of this model. Default: null
        /// </summary>
        public DefaultValueModel<string> Description { get; private set; }
        
        /// <summary>
        /// Gets a format string that will be used when displaying this model value in a template. Default: null
        /// </summary>
        public DefaultValueModel<string> DisplayFormatString { get; private set; }
        
        /// <summary>
        /// Gets the display name of this model value. Used in templates and Html.Label/LabelFor to generate the label text. Default: null
        /// </summary>
        public DefaultValueModel<string> DisplayName { get; private set; }
        
        /// <summary>
        /// Gets a format string that will be used when editing this model value in a template. Default: null
        /// </summary>
        public DefaultValueModel<string> EditFormatString { get; private set; }
        
        /// <summary>
        /// Gets a flag which indicates that this field should not have any of its surrounding HTML (for example, a label). Often used when a template will be generating a hidden input. Default: false
        /// </summary>
        public DefaultValueModel<bool> HideSurroundingHtml { get; private set; }
        
        /// <summary>
        /// Gets a flag which indicates whether the system considers this to be a complex type (and therefore will default to the complex object template rather than the string template). Not user-settable
        /// </summary>
        public DefaultValueModel<bool> IsComplexType { get; private set; }
        
        /// <summary>
        /// Gets a flag which indicates whether the model is a nullable value type (namely, Nullable). Not user-settable
        /// </summary>
        public DefaultValueModel<bool> IsNullableValueType { get; private set; }
        
        /// <summary>
        /// Gets a flag which indicates if this value is read-only (for example, because the property does not have a setter). Default: false
        /// </summary>
        public DefaultValueModel<bool> IsReadOnly { get; private set; }
        
        /// <summary>
        /// Gets a flag which indicates if this value is required. Default: true for non-nullable value types; false for all others.
        /// </summary>
        public DefaultValueModel<bool> IsRequired { get; private set; } 
        
        /// <summary>
        /// Gets the text which should be used when attempting to display a null model. Default: null
        /// </summary>
        public DefaultValueModel<string> NullDisplayText { get; private set; }
        
        /// <summary>
        /// Gets the short display name of this mode value. Intended to be used in the title of tabular list views. If this field is null, then DisplayName should be used. Default: null
        /// </summary>
        public DefaultValueModel<string> ShortDisplayName { get; private set; }
        
        /// <summary>
        /// Gets a flag which indicates if this model should be shown in display mode. Default: true
        /// </summary>
        public DefaultValueModel<bool> ShowForDisplay { get; private set; }
        
        /// <summary>
        /// Gets a flag which indicates if this model should be shown in edit mode. Default: true
        /// </summary>
        public DefaultValueModel<bool> ShowForEdit { get; private set; }
        
        /// <summary>
        /// Gets text which should be shown for this model when summarizing what would otherwise be a complex object display. Default: see below
        /// </summary>
        public DefaultValueModel<string> SimpleDisplayText { get; private set; }
        
        /// <summary>
        /// Gets a string which indicates a hint as to what template should be used for this model. Default: null
        /// </summary>
        public DefaultValueModel<string> TemplateHint { get; private set; }
        
        /// <summary>
        /// Gets text that might be displayed as a watermark when editing this model in a text box. Default: null
        /// </summary>
        public DefaultValueModel<object> Watermark { get; private set; }
    }
}
