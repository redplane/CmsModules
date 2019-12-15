using System;
using System.Collections.Generic;
using System.Linq;
using AspNetWebShared.Constants;
using AspNetWebShared.Models.MailHosts;
using MailManager.Models.Interfaces;
using MailWeb.Converters;
using MailWeb.Models.Entities;
using MailWeb.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MailWeb.Models
{
    public class MailManagementDbContext : DbContext
    {
        #region Properties

        private readonly MailClientSetting[] _mailClientSettings;

        #endregion

        #region Constructor

        public MailManagementDbContext(DbContextOptions<MailManagementDbContext> options, IConfiguration configuration) : base(options)
        {
            var mailClientSettings = new LinkedList<MailClientSetting>();

            var initialMailClientSettings = configuration
                .GetSection("mailClientSettings")
                .Get<MailClientSettingViewModel[]>();

            if (initialMailClientSettings != null && initialMailClientSettings.Length > 0)
            {
                foreach (var initialMailClientSetting in initialMailClientSettings)
                {
                    var mailClientSetting =
                        new MailClientSetting(initialMailClientSetting.Id, initialMailClientSetting.UniqueName);
                    mailClientSetting.ClientId = initialMailClientSetting.ClientId;
                    mailClientSetting.DisplayName = initialMailClientSetting.DisplayName;
                    mailClientSetting.Timeout = initialMailClientSetting.Timeout;
                    mailClientSetting.CarbonCopies = JsonConvert
                        .DeserializeObject<MailAddressViewModel[]>(initialMailClientSetting.CarbonCopies);
                    mailClientSetting.BlindCarbonCopies = JsonConvert
                        .DeserializeObject<MailAddressViewModel[]>(initialMailClientSetting.BlindCarbonCopies);
                    mailClientSetting.MailHost = JsonConvert
                        .DeserializeObject<IMailHost>(initialMailClientSetting.MailHost, new MailHostConverter());

                    mailClientSettings.AddLast(mailClientSetting);
                }
            }

            _mailClientSettings = mailClientSettings.ToArray();
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

            mailClientSetting.HasData(_mailClientSettings);
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