using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseDefaultModelBinder:DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Trace.Write(string.Format("BINDMODEL ModelName:{0}, ModelType:{1}", bindingContext.ModelName, bindingContext.ModelType), "Selected");
            var result = base.BindModel(controllerContext, bindingContext);
            return result;
        }

        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            Trace.Write("TRYING NEW PROPERTY " + propertyDescriptor.Name.ToUpper(), "Warn");
            Trace.Write(string.Format("BINDPROPERTY ModelName:{0} PropertyDescriptor:{1}", bindingContext.ModelName, propertyDescriptor.Name), "Selected");
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            Trace.Write(string.Format("CREATEMODEL ModelName:{0} ModelType:{1}", bindingContext.ModelName, modelType.Name), "Selected");
            object result = base.CreateModel(controllerContext, bindingContext, modelType);
            return result;
        }

        protected override PropertyDescriptorCollection GetModelProperties(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = base.GetModelProperties(controllerContext, bindingContext);
            Trace.Write(string.Format("GETMODELPROPERTIES ModelName:{0} PropertyCount:{1}", bindingContext.ModelName, result.Count), "Selected");
            return result;
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            var result = base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
            Trace.Write(string.Format("GETPROPERTYVALUE ModelName:{0} PropertyDescriptor:{1} Result:{2}", bindingContext.ModelName, propertyDescriptor.Name, result), "Selected");
            return result;
        }

        protected override ICustomTypeDescriptor GetTypeDescriptor(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Trace.Write(string.Format("GETTYPEDESCRIPTOR ModelName:{0}", bindingContext.ModelName), "Selected");
            var result = base.GetTypeDescriptor(controllerContext, bindingContext);
            return result;
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Trace.Write(string.Format("ONMODELUPDATED ModelName:{0}", bindingContext.ModelName), "Selected");
            base.OnModelUpdated(controllerContext, bindingContext);
        }

        protected override bool OnModelUpdating(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = base.OnModelUpdating(controllerContext, bindingContext);
            Trace.Write(string.Format("ONMODELUPDATING ModelName:{0} IsUpdating:{1}", bindingContext.ModelName, result), "Selected");
            return result;
        }

        protected override void OnPropertyValidated(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            Trace.Write(string.Format("ONPROPERTYVALIDATED ModelName:{0} PropertyDescriptor:{1} Value:{2}", bindingContext.ModelName, propertyDescriptor.Name, value), "Selected");
            base.OnPropertyValidated(controllerContext, bindingContext, propertyDescriptor, value);
        }

        protected override bool OnPropertyValidating(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            var result = base.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, value);
            Trace.Write(string.Format("ONPROPERTYVALIDATING ModelName:{0} PropertyDescriptor:{1} Value:{2} IsValid{3}", bindingContext.ModelName, propertyDescriptor.Name, value, result.ToString()), "Selected");
            return result;
        }

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            Trace.Write(string.Format("SETPROPERTY ModelName:{0} PropertyDescriptor:{1} Value:{2}", bindingContext.ModelName, propertyDescriptor.Name, value), "Selected");
            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
