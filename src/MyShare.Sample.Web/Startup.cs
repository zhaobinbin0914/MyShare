using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyShare.Kernel.Commands;
using MyShare.Kernel.Events;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Caching;
using MyShare.Sample;
using System.Reflection;
using System.Linq;
using MyShare.Kernel.Messages;
using MyShare.Kernel.Routing;
using Microsoft.AspNetCore.Http;
using MyShare.Sample.Handlers;
using MyShare.Sample.Query;
using ISession = MyShare.Kernel.Domain.ISession;

namespace MyShare.Sample.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            //Add Cqrs services
            services.AddSingleton(new Router());
            services.AddSingleton<ICommandSender>(y => y.GetService<Router>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<Router>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>());
            services.AddSingleton<IEventStore, InMemoryEventStore>();
            services.AddSingleton<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            services.AddScoped<ISession, Session>();

            services.AddTransient<IQueryModelFacade, QueryModelFacade>();

            //Scan for commandhandlers and eventhandlers
            services.Scan(scan => scan
                .FromAssemblies(typeof(InventoryCommandHandlers).GetTypeInfo().Assembly)
                    .AddClasses(classes => classes.Where(x => {
                        var allInterfaces = x.GetInterfaces();
                        return
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IHandler<>)) ||
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableHandler<>));
                    }))
                    .AsSelf()
                    .WithTransientLifetime()
            );
            // Add framework services.
            services.AddMvc();

            //Register routes
            var serviceProvider = services.BuildServiceProvider();
            var registrar = new RouteRegistrar(new Provider(serviceProvider));
            registrar.Register(typeof(InventoryCommandHandlers));

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }


    //This makes scoped services work inside router.
    public class Provider : IServiceProvider
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public Provider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _contextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
        }

        public object GetService(Type serviceType)
        {
            return _contextAccessor?.HttpContext?.RequestServices.GetService(serviceType) ??
                   _serviceProvider.GetService(serviceType);
        }
    }
}
