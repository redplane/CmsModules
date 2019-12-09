using MailServices.Models.Implementations;
using MailServices.Models.Interfaces;
using MailServices.Services.Implementations;
using MailServices.Services.Interfaces;
using MailWeb.Models;
using MailWeb.Models.Interfaces;
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
        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            
            var outlookMailServiceSetting = new SmtpMailServiceSetting("Outlook", "Sodakoq profile");
            outlookMailServiceSetting.Timeout = 180;
            outlookMailServiceSetting.HostName = "mail.sodakoqdelivery.my";
            outlookMailServiceSetting.Port = 587;
            outlookMailServiceSetting.Ssl = false;
            outlookMailServiceSetting.Username = "info@sodakoqdelivery.my";
            outlookMailServiceSetting.Password = "Msuk@2020";

            var mailGunServiceSetting = new MailGunServiceSetting("MailGun", "MailGun", 
                "sandboxe98d1e4fbfe64918ac80cb70af697266.mailgun.org", "key-8ed73db400ce5675db06685ab5265384");
            mailGunServiceSetting.Timeout = 180;

            services.AddSingleton<ISmtpMailServiceSetting, SmtpMailServiceSetting>(options => outlookMailServiceSetting);
            services.AddSingleton<IMailGunServiceSetting, MailGunServiceSetting>(options => mailGunServiceSetting);

            services.AddScoped<IMailService, OutlookMailService>();
            services.AddScoped<IMailService, MailGunService>();
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

        #endregion
    }
}