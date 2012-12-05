using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.Tab
{
    public class Metadata : AspNetTab, IDocumentation, ITabSetup, ITabLayout, IKey
    {
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(150);
                    r.Cell(1).WidthInPixels(180);
                    r.Cell(2).WidthInPixels(180);
                    r.Cell(3).WidthInPercent(15);
                    r.Cell(4).WidthInPercent(15);
                    r.Cell(5);
                }).Build();

        public override string Name
        {
            get { return "Metadata"; }
        }

        public string Key
        {
            get { return "glimpse_metadata"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Plugin/Metadata"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<View.Render.Message>();
        }
        
        public object GetLayout()
        {
            return Layout;
        }

        public override object GetData(ITabContext context)
        { 
            var viewRenderMessages = context.GetMessages<View.Render.Message>(); 

            var metadataResults = new List<MetadataItemModel>();
            if (viewRenderMessages != null)
            {
                foreach (var viewRenderMessage in viewRenderMessages)
                { 
                    var metadata = viewRenderMessage.ModelMetadata;

                    var item = new MetadataItemModel();
                    var propertyMetadata = new List<MetadataPropertyItemModel>();

                    item.Action = viewRenderMessage.ActionName;
                    item.Controller = viewRenderMessage.ControllerName;
                    if (metadata != null)
                    {
                        item.ModelMetadata = ProcessMetaData(metadata);
                        item.Name = metadata.PropertyName;
                        item.DisplayName = metadata.GetDisplayName();
                        item.Type = metadata.ModelType;
                        if (metadata.Properties != null)
                        {
                            item.PropertyMetadata = propertyMetadata;
                            foreach (var metadataProperty in metadata.Properties)
                            { 
                                propertyMetadata.Add(new MetadataPropertyItemModel { Name = metadataProperty.PropertyName, DisplayName = metadataProperty.GetDisplayName(), Metadata = ProcessMetaData(metadataProperty), Type = metadataProperty.ModelType });
                            } 
                        }
                    }

                    metadataResults.Add(item);
                }
            }

            return metadataResults;
        }

        private MetadataContentModel ProcessMetaData(ModelMetadata metadata)
        {
            var model = new MetadataContentModel();
            model.ConvertEmptyStringToNull.Value = metadata.ConvertEmptyStringToNull;
            model.DataTypeName.Value = metadata.DataTypeName;
            model.Description.Value = metadata.Description;
            model.DisplayFormatString.Value = metadata.DisplayFormatString;
            model.DisplayName.Value = metadata.DataTypeName;
            model.HideSurroundingHtml.Value = metadata.HideSurroundingHtml;
            model.IsComplexType.Value = metadata.IsComplexType;
            model.IsNullableValueType.Value = metadata.IsNullableValueType;
            model.IsReadOnly.Value = metadata.IsReadOnly;
            model.IsRequired.Value = metadata.IsRequired;
            model.IsRequired.Default = metadata.IsNullableValueType;
            model.NullDisplayText.Value = metadata.NullDisplayText;
            model.ShortDisplayName.Value = metadata.ShortDisplayName;
            model.ShowForDisplay.Value = metadata.ShowForDisplay;
            model.ShowForEdit.Value = metadata.ShowForEdit;
            model.SimpleDisplayText.Value = metadata.SimpleDisplayText;
            model.TemplateHint.Value = metadata.TemplateHint;
            model.Watermark.Value = metadata.Watermark;  

            return model;
        }
    }
}
