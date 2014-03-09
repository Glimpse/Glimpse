using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace WingtipToys.DataBindingTests
{
    public class CustomValueProvider : SimpleValueProvider
    {
        public CustomValueProvider(ModelBindingExecutionContext context) : base(context)
        {

        }
        protected override object FetchValue(string key)
        {
            return "custom value";
        }
    }
}