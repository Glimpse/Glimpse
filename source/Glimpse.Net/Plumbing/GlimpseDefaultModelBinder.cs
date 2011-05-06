using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Mvc;
using Glimpse.Net.Extensions;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseDefaultModelBinder:DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var store = controllerContext.BinderStore();

            if (!store.CurrentProperty.Name.Equals(bindingContext.ModelName))
            {
                store.MemberOf = "";
                store.CurrentProperty = new GlimpseModelBoundProperties { Name = bindingContext.ModelName, Type = bindingContext.ModelType };
            }

            Trace.Write(string.Format("BINDMODEL ModelName:{0}, ModelType:{1}", bindingContext.ModelName, bindingContext.ModelType), "Selected");
            var result = base.BindModel(controllerContext, bindingContext);
            return result;
        }

        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            var store = controllerContext.BinderStore();
            store.CurrentProperty = new GlimpseModelBoundProperties { Name = propertyDescriptor.Name, Type = propertyDescriptor.PropertyType };

            Trace.Write("TRYING NEW PROPERTY " + propertyDescriptor.Name.ToUpper(), "Warn");
            Trace.Write(string.Format("BINDPROPERTY ModelName:{0} ({2}) PropertyDescriptor:{1}", bindingContext.ModelName, propertyDescriptor.Name, propertyDescriptor.PropertyType), "Selected");
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var store = controllerContext.BinderStore();
            store.MemberOf = store.CurrentProperty.Name;

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
            //Trace.Write(string.Format("GETPROPERTYVALUE ModelName:{0} PropertyDescriptor:{1} Result:{2}", bindingContext.ModelName, propertyDescriptor.Name, result), "Selected");
            return result;
        }

        protected override ICustomTypeDescriptor GetTypeDescriptor(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //Trace.Write(string.Format("GETTYPEDESCRIPTOR ModelName:{0}", bindingContext.ModelName), "Selected");
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
            //var store = controllerContext.BinderStore();
            //store.CurrentProperty.IsValid = true;

            Trace.Write(string.Format("ONPROPERTYVALIDATED ModelName:{0} PropertyDescriptor:{1} Value:{2}", bindingContext.ModelName, propertyDescriptor.Name, value), "Selected");
            base.OnPropertyValidated(controllerContext, bindingContext, propertyDescriptor, value);
        }

        protected override bool OnPropertyValidating(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            var result = base.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, value);
            Trace.Write(string.Format("ONPROPERTYVALIDATING ModelName:{0} PropertyDescriptor:{1} Value:{2} IsValid{3}", bindingContext.ModelName, propertyDescriptor.Name, value, result), "Selected");
            return result;
        }

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            Trace.Write(string.Format("SETPROPERTY ModelName:{0} PropertyDescriptor:{1} Value:{2}", bindingContext.ModelName, propertyDescriptor.Name, value), "Selected");
            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
