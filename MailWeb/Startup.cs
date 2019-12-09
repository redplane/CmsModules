using MailServices.Models.Implementations;
using MailServices.Models.Interfaces;
using MailServices.Services.Implementations;
using MailServices.Services.Interfaces;
using MailWeb.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var outlookMailServiceSettings = new SmtpMailServiceSetting("Outlook", "Sodakoq profile");
            outlookMailServiceSettings.Timeout = 180;
            outlookMailServiceSettings.HostName = "mail.sodakoqdelivery.my";
            outlookMailServiceSettings.Port = 587;
            outlookMailServiceSettings.Ssl = true;
            outlookMailServiceSettings.Username = "info@sodakoqdelivery.my";
            outlookMailServiceSettings.Password = "Msuk@2020";

            services.AddSingleton<ISmtpMailServiceSetting, SmtpMailServiceSetting>(options => outlookMailServiceSettings);

            services.AddScoped<IMailService, OutlookMailService>();
            services.AddScoped<IMailManagerService, MailManagerService>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}