using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Model;

#if MVC2
using Glimpse.Mvc2.Backport;
#endif
#if MVC3
using Glimpse.Mvc3.Backport;
#endif

namespace Glimpse.Mvc.Tab
{
    public class ModelBinding : AspNetTab, ITabSetup, IKey, IDocumentation
    {
        public override string Name
        {
            get { return "Model Binding"; }
        }

        public string Key
        {
            get { return "glimpse_binding"; }
        }

        public string DocumentationUri
        {
            get { return "http://getGlimpse.com/Help/Model-Binding-Tab"; }
        }

        public override object GetData(ITabContext context)
        {
            var models = GetStack(context.TabStore).Where(i => i.IsBound).OrderBy(model => model.ParameterName);

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
            context.MessageBroker.Subscribe<ValueProvider<IEnumerableValueProvider>.GetValue.Message>(message => UpdateModelBinding(message, context));
            context.MessageBroker.Subscribe<ValueProvider<IEnumerableValueProvider>.ContainsPrefix.Message>(message => UpdateModelBinding(message, context));
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
                            model.ValueProviderActivity.Select(vp => new { vp.ValueProvider, vp.IsMatch }).ToArray()
                        });

                if (model.Properties.Count > 0)
                {
                    var namePrefixToUse = namePrefix;
                    if (string.IsNullOrEmpty(namePrefixToUse))
                    {
                        namePrefixToUse = model.ParameterName + ".";
                    }

                    FormatTable(table, model.Properties, ordinalString + ".", namePrefixToUse);
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
                else if ((model.ParameterName + "[0].key").Equals(parameter) || (model.ParameterName + ".index").Equals(parameter))
                {
                    // we skip these special dictionary related model entries and return a "throw-away" model, meaning that we won't add it to the stack
                    return new ModelBindingModel(parameter);
                }
                else
                {
                    // although the name of the last one on the stack doesn't seem to match the parameter, it's still possible
                    // that the model we need is there. This can happen when dictionaries and arrays are involved, since adding keys to 
                    // those, will result in adding that key/index as a model and later that model is bound, but it will never be
                    // bound as a property, since it isn't one. This means that those keys/indexes will never be popped of the stack and added
                    // to their model being the dictionary/array. So we need to move through the stack as long as the name of the model
                    // preceding it start with "{ParameterName}." or "{ParameterName}[digits}" in case we are dealing with an array 
                    // and adding all those intermediate models as "properties" until we reach the model we are looking for
                    string regexPattern = string.Format(@"^{0}\..+|^{0}\[\d+\]", parameter);
                    Regex regex = new Regex(regexPattern, RegexOptions.Compiled);
                    if (regex.IsMatch(model.ParameterName))
                    {
                        List<ModelBindingModel> possiblePropertiesOfRequestedModel = new List<ModelBindingModel>();
                        model = stack.Pop();
                        while (regex.IsMatch(model.ParameterName))
                        {
                            possiblePropertiesOfRequestedModel.Insert(0, model); 
                            model = stack.Pop();
                        }

                        // The model we have now should be the one we were looking for in the first place
                        if (model.ParameterName.Equals(parameter))
                        {
                            foreach (var modelProperty in possiblePropertiesOfRequestedModel)
                            {
                                model.Properties.Add(modelProperty);
                            }

                            stack.Push(model); // we put the requested model back on the stack as if it was found there in the first place

                            return model;
                        }
                        else
                        {
                            // This should not happen, but in case it does, we undo our popping above
                            foreach (var possiblePropertyOfRequestedModel in possiblePropertiesOfRequestedModel)
                            {
                                stack.Push(possiblePropertyOfRequestedModel);
                            }

                            stack.Push(model); // we put the requested model back on the stack as if it was found there in the first place
                        }
                    }
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