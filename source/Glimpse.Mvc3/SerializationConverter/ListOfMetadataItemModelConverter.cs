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

            var section = new TabObject();
            section.AddRow().Key("ConvertEmptyStringToNull").Value(metadata.ConvertEmptyStringToNull.Value).StrongIf(!metadata.ConvertEmptyStringToNull.IsDefault());
            section.AddRow().Key("DataTypeName").Value(metadata.DataTypeName.Value).StrongIf(!metadata.DataTypeName.IsDefault());
            section.AddRow().Key("Description").Value(metadata.Description.Value).StrongIf(!metadata.Description.IsDefault());
            section.AddRow().Key("DisplayFormatString").Value(metadata.DisplayFormatString.Value).StrongIf(!metadata.DisplayFormatString.IsDefault());
            section.AddRow().Key("DisplayName").Value(metadata.DisplayName.Value).StrongIf(!metadata.DisplayName.IsDefault());
            section.AddRow().Key("EditFormatString").Value(metadata.EditFormatString.Value).StrongIf(!metadata.EditFormatString.IsDefault());
            section.AddRow().Key("HideSurroundingHtml").Value(metadata.HideSurroundingHtml.Value).StrongIf(!metadata.HideSurroundingHtml.IsDefault());
            section.AddRow().Key("IsComplexType").Value(metadata.IsComplexType.Value).StrongIf(!metadata.IsComplexType.IsDefault());
            section.AddRow().Key("IsNullableValueType").Value(metadata.IsNullableValueType.Value).StrongIf(!metadata.IsNullableValueType.IsDefault());
            section.AddRow().Key("IsReadOnly").Value(metadata.IsReadOnly.Value).StrongIf(!metadata.IsReadOnly.IsDefault());
            section.AddRow().Key("IsRequired").Value(metadata.IsRequired.Value).StrongIf(!metadata.IsRequired.IsDefault());
            section.AddRow().Key("NullDisplayText").Value(metadata.NullDisplayText.Value).StrongIf(!metadata.NullDisplayText.IsDefault());
            section.AddRow().Key("Order").Value(metadata.Order.Value).StrongIf(!metadata.Order.IsDefault());
            section.AddRow().Key("ShortDisplayName").Value(metadata.ShortDisplayName.Value).StrongIf(!metadata.ShortDisplayName.IsDefault());
            section.AddRow().Key("ShowForDisplay").Value(metadata.ShowForDisplay.Value).StrongIf(!metadata.ShowForDisplay.IsDefault());
            section.AddRow().Key("ShowForEdit").Value(metadata.ShowForEdit.Value).StrongIf(!metadata.ShowForEdit.IsDefault());
            section.AddRow().Key("SimpleDisplayText").Value(metadata.SimpleDisplayText.Value).StrongIf(!metadata.SimpleDisplayText.IsDefault());
            section.AddRow().Key("TemplateHint").Value(metadata.TemplateHint.Value).StrongIf(!metadata.TemplateHint.IsDefault());
            section.AddRow().Key("Watermark").Value(metadata.Watermark.Value).StrongIf(!metadata.Watermark.IsDefault());
             
            return section;
        }
    }
}
