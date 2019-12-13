using System;
using MailManager.Models.Interfaces;
using MailWeb.Constants;
using MailWeb.Models.Entities;
using MailWeb.Models.MailHosts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MailWeb.Models
{
    public class MailManagementDbContext : DbContext
    {
        #region Constructor

        public MailManagementDbContext(DbContextOptions<MailManagementDbContext> options) : base(options)
        {
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddMailClientSettingTable(modelBuilder);
            AddClientSettingTable(modelBuilder);
        }

        #endregion

        #region Properties

        public virtual DbSet<MailClientSetting> MailClientSettings { get; set; }

        public virtual DbSet<ClientSetting> ClientSettings { get; set; }

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
            var clientSetting = modelBuilder.Entity<ClientSetting>();
            clientSetting.HasKey(x => x.Id);

            clientSetting.Property(x => x.Name)
                .IsRequired();

            clientSetting.HasIndex(x => x.Name)
                .IsUnique();
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