using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Glimpse.Mvc.AlternateType;

namespace Glimpse.Mvc.Model
{
    public class ModelBindingModel
    {
        public ModelBindingModel(string parameter)
        {
            ParameterName = parameter;
            ValueProviderActivity = new List<ValueProviderModel>();
            Properties = new List<ModelBindingModel>();
            IsBound = false;
        }

        public string ParameterName { get; private set; }

        public Type ParameterType { get; set; }

        public Type ModelBinderType { get; set; }

        public CultureInfo Culture { get; set; }

        public string AttemptedValue { get; set; }

        public object RawValue { get; set; }

        public IList<ModelBindingModel> Properties { get; set; }

        public IList<ValueProviderModel> ValueProviderActivity { get; set; }

        internal bool IsBound { get; set; }

        public void Add<T>(ValueProvider<T>.ContainsPrefix.Message message) where T : class
        {
            if (!ValueProviderActivity.Any(vp => vp.IsMatch))
            {
                ValueProviderActivity.Add(new ValueProviderModel(message.ValueProviderType, message.IsMatch));    
            }
        }

        public void Add<T>(ValueProvider<T>.GetValue.Message message) where T : class
        {
            if (message.IsFound)
            {
                AttemptedValue = message.AttemptedValue;
                Culture = message.Culture;
            }
        }

        public void Bound(ModelBinder.BindModel.Message message)
        {
            ModelBinderType = message.ModelBinderType;
            ParameterType = message.ModelType;
            RawValue = message.RawValue;
            IsBound = true;
        }

        public class ValueProviderModel
        {
            public ValueProviderModel(Type valueProvider, bool isMatch)
            {
                ValueProvider = valueProvider;
                IsMatch = isMatch;
            }

            public Type ValueProvider { get; set; }

            public bool IsMatch { get; set; }
        }
    }
}