using System.Web.Http;
using Glimpse.Core.Extensibility;
using System;

namespace Glimpse.WebApi.WebHost.Inspector
{
    public class ExecutionInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            var logger = context.Logger;

            context.CreateAlternateService<System.Web.Http.Controllers.IHttpActionInvoker, Glimpse.WebApi.Core.AlternateType.IHttpActionInvoker>();
            context.CreateAlternateService<System.Web.Http.Controllers.IHttpActionSelector, Glimpse.WebApi.Core.AlternateType.IHttpActionSelector>();
            context.CreateAlternateService<System.Web.Http.Dispatcher.IHttpControllerActivator, Glimpse.WebApi.Core.AlternateType.IHttpControllerActivator>();

            context.CreateAlternateService<System.Web.Http.Dispatcher.IHttpControllerSelector, Glimpse.WebApi.Core.AlternateType.IHttpControllerSelector>();
            context.CreateAlternateService<System.Web.Http.Dispatcher.IHttpControllerTypeResolver, Glimpse.WebApi.Core.AlternateType.IHttpControllerTypeResolver>();


            context.CreateAlternateService<System.Web.Http.Controllers.IActionHttpMethodProvider, Glimpse.WebApi.Core.AlternateType.IActionHttpMethodProvider>();
            context.CreateAlternateService<System.Web.Http.Controllers.IActionResultConverter, Glimpse.WebApi.Core.AlternateType.IActionResultConverter>();
            context.CreateAlternateService<System.Web.Http.Controllers.IActionValueBinder, Glimpse.WebApi.Core.AlternateType.IActionValueBinder>();

            context.CreateAlternateService<System.Web.Http.Dispatcher.IAssembliesResolver, Glimpse.WebApi.Core.AlternateType.IAssembliesResolver>();
            //context.CreateAlternateService<System.Net.Http.Formatting.IContentNegotiator, Glimpse.WebApi.Core.AlternateType.IContentNegotiator>();

            //context.CreateAlternateService<System.Web.Http.Description.IApiExplorer, Glimpse.WebApi.AlternateType.IApiExplorer>();

            //context.CreateAlternateService<System.Web.Http.Validation.IBodyModelValidator, Glimpse.WebApi.AlternateType.IBodyModelValidator>();
            //context.CreateAlternateService<System.Web.Http.Filters.IFilterProvider, Glimpse.WebApi.AlternateType.IFilterProvider>();
            //context.CreateAlternateService<System.Web.Http.Hosting.IHostBufferPolicySelector, Glimpse.WebApi.AlternateType.IHostBufferPolicySelector>();
            //context.CreateAlternateService<System.Web.Http.ModelBinding.ModelBinderProvider, Glimpse.WebApi.AlternateType.ModelBinderProvider>();
            //context.CreateAlternateService<System.Web.Http.Metadata.ModelMetadataProvider, Glimpse.WebApi.AlternateType.ModelMetadataProvider>();
            //context.CreateAlternateService<System.Web.Http.Validation.ModelValidatorProvider, Glimpse.WebApi.AlternateType.ModelValidatorProvider>();
            //context.CreateAlternateService<System.Web.Http.ValueProviders.ValueProviderFactory, Glimpse.WebApi.AlternateType.ValueProviderFactory>();

            //context.CreateAlternateService<System.Web.Http.Tracing.ITraceManager, Glimpse.WebApi.AlternateType.ITraceManager>();
            //context.CreateAlternateService<System.Web.Http.Tracing.ITraceWriter, Glimpse.WebApi.AlternateType.ITraceWriterservice>();
            //context.CreateAlternateService<System.Web.Http.Description.IDocumentationProvider, Glimpse.WebApi.AlternateType.IDocumentationProvider>();
        }

    }

    public static class IInspectorContextExtensions
    {
        public static void CreateAlternateService<TOriginal, TAlternate>(this IInspectorContext context)
            where TOriginal : class
            where TAlternate : AlternateType<TOriginal>
        {
            TOriginal originalObj = (TOriginal)GlobalConfiguration.Configuration.Services.GetService(typeof(TOriginal));
            var alternateImplementation = (TAlternate)Activator.CreateInstance(typeof(TAlternate), context.ProxyFactory);
            TOriginal newObj;

            if (alternateImplementation.TryCreate(originalObj, out newObj))
            {
                GlobalConfiguration.Configuration.Services.Replace(typeof(TOriginal), newObj);
                context.Logger.Debug(Resources.ServicesSetup, typeof(TOriginal).GetType());
            }
        }

    }
}