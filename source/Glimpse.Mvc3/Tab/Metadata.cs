using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.Tab
{
    public class Metadata : AspNetTab, IDocumentation, ITabSetup
    {
        public override string Name
        {
            get { return "Metadata"; }
        }

        public string DocumentationUri
        {
            // TODO: Update to proper Uri
            get { return "http://localhost/someUrl"; }
        }

        public void Setup(ITabSetupContext context)
        { 
            context.MessageBroker.Subscribe<View.Render.Message>(message => Persist(message, context));
        }

        public override object GetData(ITabContext context)
        { 
            var viewRenderMessages = context.TabStore.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName); 

            var metadataResults = new List<MetadataItemModel>();
            if (viewRenderMessages != null)
            {
                foreach (var viewRenderMessage in viewRenderMessages)
                {
                    var viewContext = viewRenderMessage.Input.ViewContext;
                    var metadata = viewContext.ViewData.ModelMetadata;

                    var item = new MetadataItemModel();
                    var propertyMetadata = new List<MetadataPropertyItemModel>();

                    item.Action = viewContext.Controller.ValueProvider.GetValue("action").RawValue.ToStringOrDefault();
                    item.Controller = viewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToStringOrDefault();
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

        internal static void Persist<T>(T message, ITabSetupContext context)
        {
            var tabStore = context.GetTabStore();
            var key = typeof(T).FullName;

            if (!tabStore.Contains(key))
            {
                tabStore.Set(key, new List<T>());
            }

            var messages = tabStore.Get<IList<T>>(key);

            messages.Add(message);
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
