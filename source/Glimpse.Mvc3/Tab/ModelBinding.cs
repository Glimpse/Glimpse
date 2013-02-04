using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.AlternateType;

#if MVC2
using Glimpse.Mvc2.Backport;
#endif

namespace Glimpse.Mvc.Tab
{
    public class ModelBinding : AspNetTab, ITabSetup, IKey
    {
        public override string Name
        {
            get { return "Model Binding"; }
        }

        public string Key
        {
            get { return "glimpse_binding"; }
        }

        public override object GetData(ITabContext context)
        {
            var model = GetModel(context.TabStore);

            if (model.Properties.Count == 0)
            {
                return null;
            }

            var table = new List<object[]> { new object[] { "Ordinal", "Model Binder", "Property/Parameter", "Type", "Attempted Value Providers", "Attempted Value", "Culture", "Raw Value" } };

            var ordinal = 0;

            foreach (var property in model.Properties)
            {
                var providers = new List<object[]> { new object[] { "Provider", "Successful" } };
                providers.AddRange(property.NotFoundIn.Select(type => new object[] { type, false }));

                if (property.FoundIn != null)
                {
                    providers.Add(new object[] { property.FoundIn, true, "selected" });
                }

                table.Add(
                    new[] 
                    {  
                        ordinal++,
                        property.ModelBinderType,
                        string.IsNullOrEmpty(property.MemberOf) ? property.Name : property.MemberOf + "." + property.Name,
                        property.Type, 
                        providers, 
                        property.AttemptedValue, 
                        property.Culture != null ? property.Culture.DisplayName : null,
                        property.RawValue,
                        string.IsNullOrEmpty(property.MemberOf) ? string.Empty : "quiet"
                });
            }

            return table;
        }

        public void Setup(ITabSetupContext context)
        {
            context.MessageBroker.Subscribe<ValueProvider<IUnvalidatedValueProvider>.ContainsPrefix.Message>(message => UpdateModelBinding(message, context));
            context.MessageBroker.Subscribe<ValueProvider<IValueProvider>.ContainsPrefix.Message>(message => UpdateModelBinding(message, context));
            context.MessageBroker.Subscribe<ValueProvider<IUnvalidatedValueProvider>.GetValue.Message>(message => UpdateModelBinding(message, context));
            context.MessageBroker.Subscribe<ValueProvider<IValueProvider>.GetValue.Message>(message => UpdateModelBinding(message, context));
            context.MessageBroker.Subscribe<ModelBinder.BindModel.Message>(message => UpdateModelBinding(message, context));
            context.MessageBroker.Subscribe<ModelBinder.BindProperty.Message>(message => UpdateModelBinding(message, context));
        }

        private void UpdateModelBinding<T>(ValueProvider<T>.ContainsPrefix.Message message, ITabSetupContext context) where T : class
        {
            if (!message.IsMatch)
            {
                var model = GetModel(context.GetTabStore());
                model.CurrentProperty.NotFoundIn.Add(message.ValueProviderType);
            }
        }

        private void UpdateModelBinding<T>(ValueProvider<T>.GetValue.Message message, ITabSetupContext context) where T : class
        {
            if (message.IsFound)
            {
                var model = GetModel(context.GetTabStore());
                var currentProperty = model.CurrentProperty;
                currentProperty.FoundIn = message.ValueProviderType;
                currentProperty.AttemptedValue = message.AttemptedValue;
                currentProperty.Culture = message.Culture;
            }
        }

        private void UpdateModelBinding(ModelBinder.BindModel.Message message, ITabSetupContext context)
        {
            var model = GetModel(context.GetTabStore());
            var currentProperty = model.CurrentProperty;

            currentProperty.ModelBinderType = message.ModelBinderType;
            currentProperty.RawValue = message.RawValue;

            if (!currentProperty.Name.Equals(message.ModelName))
            {
                model.MemberOf = string.Empty;
                model.CurrentProperty = new ModelBindingPropertyModel { Name = message.ModelName, Type = message.ModelType };
            }
        }

        private void UpdateModelBinding(ModelBinder.BindProperty.Message message, ITabSetupContext context)
        {
            var model = GetModel(context.GetTabStore());
            model.CurrentProperty = new ModelBindingPropertyModel { Name = message.Name, Type = message.Type, ModelBinderType = message.ModelBinderType };
        }

        private ModelBindingModel GetModel(IDataStore tabStore)
        {
            ModelBindingModel model;

            if (tabStore.Contains<ModelBindingModel>())
            {
                model = tabStore.Get<ModelBindingModel>();
            }
            else
            {
                model = new ModelBindingModel();
                tabStore.Set(model);
            }

            return model;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This type is still a work in progress. It will be moved soon.")]
    public class ModelBindingPropertyModel
    {
        public ModelBindingPropertyModel()
        {
            NotFoundIn = new HashSet<Type>();
        }

        public Type ModelBinderType { get; set; }

        public string Name { get; set; }
        
        public Type Type { get; set; }
        
        public HashSet<Type> NotFoundIn { get; set; }
        
        public Type FoundIn { get; set; }
        
        public string AttemptedValue { get; set; }
        
        public object RawValue { get; set; }
        
        public string MemberOf { get; set; }
        
        public CultureInfo Culture { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This type is still a work in progress. It will be moved soon.")]
    public class ModelBindingModel
    {
        private ModelBindingPropertyModel currentProperty;

        public ModelBindingModel()
        {
            Properties = new List<ModelBindingPropertyModel>();
        }
        
        public List<ModelBindingPropertyModel> Properties { get; set; }

        public string MemberOf { get; set; }

        public ModelBindingPropertyModel CurrentProperty
        {
            get
            {
                return currentProperty ?? (currentProperty = new ModelBindingPropertyModel { MemberOf = MemberOf, Name = string.Empty });
            }

            set
            {
                value.MemberOf = MemberOf;
                currentProperty = value;
                Properties.Add(value);
            }
        }
    }
}