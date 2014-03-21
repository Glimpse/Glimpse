using System.Web.Http;
using Glimpse.Core.Extensibility;
using System;

namespace Glimpse.WebApi.Inspector
{
    public class ExecutionInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            var logger = context.Logger;

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new GlimpseHandler());

            var dummyAuthorizationFilter = new DummyAuthorizationFilter(logger);
            var alternateAuthorizationFilterImplementation = new Glimpse.WebApi.AlternateType.AuthorizationFilterAttribute(context.ProxyFactory);
            System.Web.Http.Filters.AuthorizationFilterAttribute newAuthorizationFilter;

            if (alternateAuthorizationFilterImplementation.TryCreate(dummyAuthorizationFilter, out newAuthorizationFilter))
            {
                GlobalConfiguration.Configuration.Filters.Add(newAuthorizationFilter);
                logger.Debug(Resources.ActionFilterSetup, dummyAuthorizationFilter.GetType());
            }

            var dummyActionFilter = new DummyActionFilter(logger);
            var alternateActionFilterImplementation = new Glimpse.WebApi.AlternateType.ActionFilterAttribute(context.ProxyFactory);
            System.Web.Http.Filters.ActionFilterAttribute newActionFilter;

            if (alternateActionFilterImplementation.TryCreate(dummyActionFilter, out newActionFilter))
            {
                GlobalConfiguration.Configuration.Filters.Add(newActionFilter);
                logger.Debug(Resources.ActionFilterSetup, dummyActionFilter.GetType());
            }

            context.CreateAlternateService<System.Web.Http.Controllers.IHttpActionInvoker, Glimpse.WebApi.AlternateType.IHttpActionInvoker>();
            context.CreateAlternateService<System.Web.Http.Controllers.IHttpActionSelector, Glimpse.WebApi.AlternateType.IHttpActionSelector>();
            context.CreateAlternateService<System.Web.Http.Dispatcher.IHttpControllerActivator, Glimpse.WebApi.AlternateType.IHttpControllerActivator>();

            context.CreateAlternateService<System.Web.Http.Dispatcher.IHttpControllerSelector, Glimpse.WebApi.AlternateType.IHttpControllerSelector>();
            context.CreateAlternateService<System.Web.Http.Dispatcher.IHttpControllerTypeResolver, Glimpse.WebApi.AlternateType.IHttpControllerTypeResolver>();


            context.CreateAlternateService<System.Web.Http.Controllers.IActionHttpMethodProvider, Glimpse.WebApi.AlternateType.IActionHttpMethodProvider>();
            context.CreateAlternateService<System.Web.Http.Controllers.IActionResultConverter, Glimpse.WebApi.AlternateType.IActionResultConverter>();
            context.CreateAlternateService<System.Web.Http.Controllers.IActionValueBinder, Glimpse.WebApi.AlternateType.IActionValueBinder>();

            //context.CreateAlternateService<System.Web.Http.Description.IApiExplorer, Glimpse.WebApi.AlternateType.IApiExplorer>();

            //context.CreateAlternateService<System.Web.Http.Dispatcher.IAssembliesResolver, Glimpse.WebApi.AlternateType.IAssembliesResolver>();
            //context.CreateAlternateService<System.Web.Http.Validation.IBodyModelValidator, Glimpse.WebApi.AlternateType.IBodyModelValidator>();
            //context.CreateAlternateService<System.Net.Http.Formatting.IContentNegotiator, Glimpse.WebApi.AlternateType.IContentNegotiator>();
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