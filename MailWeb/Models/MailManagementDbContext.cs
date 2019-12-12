using System;
using System.Reflection;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;
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
            AddBasicMailSettingTable(modelBuilder);
            AddClientSettingTable(modelBuilder);
        }

        #endregion

        #region Properties

        public virtual DbSet<BasicMailSetting> BasicMailSettings { get; set; }

        public virtual DbSet<ClientSetting> ClientSettings { get; set; }

        #endregion

        #region Inner methods

        protected virtual void AddBasicMailSettingTable(ModelBuilder modelBuilder)
        {
            var basicMailSetting = modelBuilder.Entity<BasicMailSetting>();
            basicMailSetting.HasKey(x => x.Id);

            basicMailSetting.Property(x => x.UniqueName)
                .IsRequired();

            basicMailSetting.HasIndex(x => x.UniqueName)
                .IsUnique();

            basicMailSetting
                .Property(x => x.MailHost)
                .HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => HandleIncomingMailHost(x));

        }

        protected virtual void AddClientSettingTable(ModelBuilder modelBuilder)
        {
            var clientSetting = modelBuilder.Entity<ClientSetting>();
            clientSetting.HasKey(x => x.Id);

            clientSetting.Property(x => x.Name)
                .IsRequired();

            clientSetting.HasIndex(x => x.Name)
                .IsUnique();

            clientSetting.OwnsOne(x => x.ActiveMailService)
                .Property(x => x.Name)
                .HasColumnName("ActiveMailServiceName");

            clientSetting.OwnsOne(x => x.ActiveMailService)
                .Property(x => x.Type)
                .HasColumnName("ActiveMailServiceType");
        }

        protected virtual IMailHost HandleIncomingMailHost(string x)
        {
            var node = JObject.Parse(x);
            var nodeType = node.GetValue(nameof(IMailHost.Type));
            var szNodeType = nodeType.Value<string>();
            var mailHost = JsonConvert.DeserializeObject(x, Type.GetType(szNodeType));
            return (IMailHost) mailHost;
        }

        #endregion
    }
}