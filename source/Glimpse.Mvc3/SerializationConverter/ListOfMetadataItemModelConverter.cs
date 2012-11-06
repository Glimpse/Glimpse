using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.SerializationConverter
{
    public class ListOfMetadataItemModelConverter : SerializationConverter<List<MetadataItemModel>>
    {
        public override object Convert(List<MetadataItemModel> models)
        {
            var root = new TabSection("Category", "Controller", "Action", "Name", "Type", "Metadata"); 
            foreach (var item in models)
            {
                root.AddRow().Column("Model").Column(item.Controller).Column(item.Action).Column(item.Name).Column(item.Type).Column(ConvertMetadata(item.ModelMetadata));
                if (item.PropertyMetadata != null)
                {
                    foreach (var property in item.PropertyMetadata)
                    {
                        root.AddRow().Column("\t ↑ Property").Column(string.Empty).Column(string.Empty).Column(property.Name).Column(property.Type).Column(ConvertMetadata(property.Metadata));
                    }
                }
            } 

            return root.Build(); 
        }

        private object ConvertMetadata(MetadataContentModel metadata)
        {
            if (metadata == null)
            {
                return null;
            }

            var section = new TabSection("Key", "Value");
            section.AddRow().Column("ConvertEmptyStringToNull").Column(metadata.ConvertEmptyStringToNull.Value).StrongIf(!metadata.ConvertEmptyStringToNull.IsDefault());
            section.AddRow().Column("DataTypeName").Column(metadata.DataTypeName.Value).StrongIf(!metadata.DataTypeName.IsDefault());
            section.AddRow().Column("Description").Column(metadata.Description.Value).StrongIf(!metadata.Description.IsDefault());
            section.AddRow().Column("DisplayFormatString").Column(metadata.DisplayFormatString.Value).StrongIf(!metadata.DisplayFormatString.IsDefault());
            section.AddRow().Column("DisplayName").Column(metadata.DisplayName.Value).StrongIf(!metadata.DisplayName.IsDefault());
            section.AddRow().Column("EditFormatString").Column(metadata.EditFormatString.Value).StrongIf(!metadata.EditFormatString.IsDefault());
            section.AddRow().Column("HideSurroundingHtml").Column(metadata.HideSurroundingHtml.Value).StrongIf(!metadata.HideSurroundingHtml.IsDefault());
            section.AddRow().Column("IsComplexType").Column(metadata.IsComplexType.Value).StrongIf(!metadata.IsComplexType.IsDefault());
            section.AddRow().Column("IsNullableValueType").Column(metadata.IsNullableValueType.Value).StrongIf(!metadata.IsNullableValueType.IsDefault());
            section.AddRow().Column("IsReadOnly").Column(metadata.IsReadOnly.Value).StrongIf(!metadata.IsReadOnly.IsDefault());
            section.AddRow().Column("IsRequired").Column(metadata.IsRequired.Value).StrongIf(!metadata.IsRequired.IsDefault());
            section.AddRow().Column("NullDisplayText").Column(metadata.NullDisplayText.Value).StrongIf(!metadata.NullDisplayText.IsDefault());
            section.AddRow().Column("Order").Column(metadata.Order.Value).StrongIf(!metadata.Order.IsDefault());
            section.AddRow().Column("ShortDisplayName").Column(metadata.ShortDisplayName.Value).StrongIf(!metadata.ShortDisplayName.IsDefault());
            section.AddRow().Column("ShowForDisplay").Column(metadata.ShowForDisplay.Value).StrongIf(!metadata.ShowForDisplay.IsDefault());
            section.AddRow().Column("ShowForEdit").Column(metadata.ShowForEdit.Value).StrongIf(!metadata.ShowForEdit.IsDefault());
            section.AddRow().Column("SimpleDisplayText").Column(metadata.SimpleDisplayText.Value).StrongIf(!metadata.SimpleDisplayText.IsDefault());
            section.AddRow().Column("TemplateHint").Column(metadata.TemplateHint.Value).StrongIf(!metadata.TemplateHint.IsDefault());
            section.AddRow().Column("Watermark").Column(metadata.Watermark.Value).StrongIf(!metadata.Watermark.IsDefault());
             
            return section;
        }
    }
}
