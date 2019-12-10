using MailWeb.Models.Entities;
using MailWeb.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Models
{
    public class MailManagementDbContext : DbContext
    {
        #region Properties

        public virtual DbSet<BasicMailSetting> BasicMailSettings { get; set; }

        public virtual DbSet<ClientSetting> ClientSettings { get; set; }

        #endregion

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
                .OwnsOne(x => x.Credential)
                .Property(x => x.Username)
                .HasColumnName(nameof(SmtpCredentialValueObject.Username));

            basicMailSetting
                .OwnsOne(x => x.Credential)
                .Property(x => x.Password)
                .HasColumnName(nameof(SmtpCredentialValueObject.Password));
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

        #endregion
    }
}