using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace WingtipToys.DataBindingTests
{
    public class CustomAttribute : ValueProviderSourceAttribute
    {
        public override IValueProvider GetValueProvider(ModelBindingExecutionContext modelBindingExecutionContext)
        {
            return new CustomValueProvider(modelBindingExecutionContext);
        }
    }
}