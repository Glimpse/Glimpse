using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Model;

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
            var models = GetStack(context.TabStore).Where(i => i.IsBound).Reverse();

            if (!models.Any())
            {
                return null;
            }

            // TODO: Move this display code over to leverage assist namespace and a serialization converter.
            var table = new List<object[]> { new object[] { "Ordinal", "Parameter", "Type", "Value", "Culture", "Model Binder", "Value Providers" } };
            
            FormatTable(table, models, string.Empty, string.Empty);

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

        private void FormatTable(List<object[]> table, IEnumerable<ModelBindingModel> models, string ordinalPrefix, string namePrefix)
        {
            var innerOrdinal = 1;

            foreach (var model in models)
            {
                var ordinalString = ordinalPrefix + innerOrdinal++;
                table.Add(
                    new[]
                        {
                            ordinalString,
                            namePrefix + model.ParameterName,
                            model.ParameterType,
                            model.RawValue,
                            model.Culture,
                            model.ModelBinderType,
                            model.ValueProviderActivity.Select(vp => vp.ValueProvider).ToArray()
                        });
                if (model.Properties.Count > 0)
                {
                    FormatTable(table, model.Properties, ordinalString + ".", model.ParameterName + ".");
                }
            }
        }

        private void UpdateModelBinding<T>(ValueProvider<T>.ContainsPrefix.Message message, ITabSetupContext context) where T : class
        {
            var model = GetModel(GetStack(context.GetTabStore()), message.Prefix);
            model.Add(message);
        }

        private void UpdateModelBinding<T>(ValueProvider<T>.GetValue.Message message, ITabSetupContext context) where T : class
        {
            var model = GetModel(GetStack(context.GetTabStore()), message.Key);
            model.Add(message);
        }

        private void UpdateModelBinding(ModelBinder.BindModel.Message message, ITabSetupContext context)
        {
            var model = GetModel(GetStack(context.GetTabStore()), message.ModelName);
            model.Bound(message);
        }

        private void UpdateModelBinding(ModelBinder.BindProperty.Message message, ITabSetupContext context)
        {
            var stack = GetStack(context.GetTabStore());

            var property = stack.Pop();
            var model = stack.Peek();

            model.Properties.Add(property);
        }

        private ModelBindingModel GetModel(Stack<ModelBindingModel> stack, string parameter)
        {
            ModelBindingModel model;
            if (stack.Any())
            {
                model = stack.Peek();
                if (model.ParameterName.Equals(parameter))
                {
                    return model;
                }
            }

            model = new ModelBindingModel(parameter);
            stack.Push(model);
            return model;
        }

        private Stack<ModelBindingModel> GetStack(IDataStore tabStore)
        {
            Stack<ModelBindingModel> stack;

            if (tabStore.Contains<Stack<ModelBindingModel>>())
            {
                stack = tabStore.Get<Stack<ModelBindingModel>>();
            }
            else
            {
                stack = new Stack<ModelBindingModel>();
                tabStore.Set(stack);
            }

            return stack;
        }
    }
}