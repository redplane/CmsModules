using System.Reflection;
using CorsModule.Services.Interfaces;
using FluentValidation.AspNetCore;
using MailModule.Services.Interfaces;
using MailWeb.Constants;
using MailWeb.Cqrs;
using MailWeb.Extensions;
using MailWeb.Models;
using MailWeb.Models.Interfaces;
using MailWeb.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;

namespace MailWeb
{
    public class Startup
    {
        #region Constructor

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddScoped<IRequestProfile, RequestProfile>(serviceProvider =>
            {
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor.HttpContext;
                var tenantId = httpContext.GetTenantId();

                return new RequestProfile(tenantId);
            });

            services.AddScoped<ITenant>(options =>
            {
                var httpContextAccessor = options.GetService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor.HttpContext;

                var tenantId = httpContext.GetTenantId();
                var tenant = new Tenant(tenantId);
                return tenant;
            });

            // Add connection string into system.
            services
                .AddDbContext<SiteDbContext>(options => options
                    .UseSqlite(Configuration.GetConnectionString(ConnectionStringKeyConstants.Default)));

            services.AddScoped<SiteDbContext>();

            services.AddScoped<IMailClientsManager, MailClientsManager>();
            services.AddScoped<ICorsPoliciesManager, CorsPoliciesManager>();

            // Add mediatr.
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            // Request validation.
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddCors();
            services.AddTransient<ICorsPolicyProvider, SiteCorsPolicyProvider>();

            services
                .AddMvc()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(options =>
                {
                    var camelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ContractResolver = camelCasePropertyNamesContractResolver;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors();
            app.UseMvc();
        }

        #endregion
    }
}