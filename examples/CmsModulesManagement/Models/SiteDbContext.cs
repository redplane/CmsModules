using System;
using CmsModulesManagement.Models.Entities;
using CmsModulesShared.Constants;
using CmsModulesShared.Models.MailHosts;
using MailModule.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CmsModulesManagement.Models
{
    public class SiteDbContext : DbContext
    {
        #region Constructor

        public SiteDbContext(DbContextOptions<SiteDbContext> options) : base(options)
        {
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddMailClientSettingTable(modelBuilder);
            AddClientSettingTable(modelBuilder);
            AddCorsPoliciesTable(modelBuilder);
        }

        #endregion

        #region Properties

        public virtual DbSet<MailClientSetting> MailClientSettings { get; set; }

        public virtual DbSet<SiteSetting> ClientSettings { get; set; }

        public virtual DbSet<SiteCorsPolicy> CorsPolicies { get; set; }

        #endregion

        #region Inner methods

        protected virtual void AddMailClientSettingTable(ModelBuilder modelBuilder)
        {
            var mailClientSetting = modelBuilder.Entity<MailClientSetting>();
            mailClientSetting.HasKey(x => x.Id);

            mailClientSetting.Property(x => x.UniqueName)
                .IsRequired();

            mailClientSetting.HasIndex(x => x.UniqueName)
                .IsUnique();

            mailClientSetting
                .Property(x => x.MailHost)
                .HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => HandleIncomingMailHost(x));

            mailClientSetting.Property(x => x.CarbonCopies)
                .HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<IMailAddress[]>(x));

            mailClientSetting.Property(x => x.BlindCarbonCopies)
                .HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<IMailAddress[]>(x));
        }

        protected virtual void AddClientSettingTable(ModelBuilder modelBuilder)
        {
            var clientSetting = modelBuilder.Entity<SiteSetting>();
            clientSetting.HasKey(x => x.Id);

            clientSetting.Property(x => x.Name)
                .IsRequired();

            clientSetting.HasIndex(x => x.Name)
                .IsUnique();
        }

        protected virtual void AddCorsPoliciesTable(ModelBuilder modelBuilder)
        {
            var corsPolicies = modelBuilder.Entity<SiteCorsPolicy>();
            corsPolicies.HasKey(x => x.Id);
            corsPolicies.Property(x => x.Name)
                .IsRequired();

            corsPolicies.HasIndex(x => x.Name)
                .IsUnique();

            var textsToJsonTextConverter = new ValueConverter<string[], string>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<string[]>(v));


            var allowedHeaders = corsPolicies.Property(x => x.AllowedHeaders);
            allowedHeaders.HasConversion(textsToJsonTextConverter);

            var allowedOrigins = corsPolicies.Property(x => x.AllowedOrigins);
            allowedOrigins.HasConversion(textsToJsonTextConverter);

            var allowedMethods = corsPolicies.Property(x => x.AllowedMethods);
            allowedMethods.HasConversion(textsToJsonTextConverter);

            var allowedExposedHeaders = corsPolicies.Property(x => x.AllowedExposedHeaders);
            allowedExposedHeaders.HasConversion(textsToJsonTextConverter);
        }

        protected virtual IMailHost HandleIncomingMailHost(string x)
        {
            var node = JObject.Parse(x);
            var jNodeTypeToken = node.GetValue(nameof(IMailHost.Type));
            var nodeTypeName = jNodeTypeToken.Value<string>();

            Type mailHostType = null;
            if (MailHostKindConstants.Smtp.Equals(nodeTypeName, StringComparison.InvariantCultureIgnoreCase))
                mailHostType = typeof(SmtpHost);
            else if (MailHostKindConstants.MailGun.Equals(nodeTypeName, StringComparison.InvariantCultureIgnoreCase))
                mailHostType = typeof(MailGunHost);

            if (mailHostType != null)
            {
                var mailHost = JsonConvert.DeserializeObject(x, mailHostType);
                return (IMailHost) mailHost;
            }

            return default;
        }

        #endregion
    }
}