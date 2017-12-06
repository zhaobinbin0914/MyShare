using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel.Infrastructure;
using MyShare.Kernel.Mvc;

namespace MyShare.Kernel.Providers
{
    /// <summary>
    /// Used to add application services as controller.
    /// </summary>
    public class ApplicationServiceControllerFeatureProvider : ControllerFeatureProvider
    {
        private IServiceCollection _serviceCollection;

        public ApplicationServiceControllerFeatureProvider(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            var type = typeInfo.AsType();

            return typeof(IApplicationService).IsAssignableFrom(type) && typeInfo.IsPublic && !typeInfo.IsAbstract && !typeInfo.IsGenericType;
        }
    }
}
