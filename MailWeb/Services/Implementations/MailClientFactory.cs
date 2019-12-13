using System;
using System.Linq;
using System.Net.Http;
using MailManager.Models.Interfaces;
using MailManager.Services.Interfaces;
using MailWeb.Models;
using MailWeb.Models.Interfaces;
using MailWeb.Models.MailHosts;

namespace MailWeb.Services.Implementations
{
    public class MailClientFactory : IMailClientFactory
    {
        #region Constructor

        public MailClientFactory(
            IRequestProfile requestProfile, IHttpClientFactory httpClientFactory,
            MailManagementDbContext dbContext)
        {
            _requestProfile = requestProfile;
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        #region Properties

        private readonly MailManagementDbContext _dbContext;

        private readonly IRequestProfile _requestProfile;

        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Methods

        public IMailClient[] GetMailServices()
        {
            throw new NotImplementedException();
        }

        public IMailClient GetMailService(string uniqueName)
        {
            var mailClients = _dbContext.MailClientSettings
                .Where(x => x.UniqueName == uniqueName);

            var mailClientSetting = mailClients.FirstOrDefault();
            if (mailClientSetting == null)
                return default;

            return ToMailClient(mailClientSetting);
        }

        public IMailClient GetActiveMailClient()
        {
            // Find the client settings.
            var tenantId = _requestProfile.TenantId;

            // Find the client setting.
            var clientSetting = _dbContext.ClientSettings
                .FirstOrDefault(x => x.Id == tenantId);
            

            if (clientSetting == null)
                return null;

            var mailClientSettingUniqueName = clientSetting.ActiveMailClient;
            var mailClientSetting = _dbContext.MailClientSettings
                .FirstOrDefault(x => x.UniqueName == mailClientSettingUniqueName);

            return ToMailClient(mailClientSetting);
        }

        public void SetActiveMailClient(string uniqueName)
        {
            // Find the client settings.
            var tenantId = _requestProfile.TenantId;

            // Find the client setting.
            var clientSetting = _dbContext.ClientSettings
                .FirstOrDefault(x => x.Id == tenantId);

            if (clientSetting == null)
                return;

            // Find the mail service.
            var mailService = GetMailService(uniqueName);
            if (mailService == null)
                return;

            clientSetting.ActiveMailClient = uniqueName;
            _dbContext.SaveChanges();
        }

        protected virtual IMailClient ToMailClient(IMailClientSetting mailClientSetting)
        {
            if (mailClientSetting == null)
                return null;

            if (mailClientSetting.MailHost == null)
                return null;

            if (mailClientSetting.MailHost is SmtpHost)
                return new OutlookMailClient(mailClientSetting);

            if (mailClientSetting.MailHost is MailGunHost)
                return new MailGunClient(mailClientSetting, _httpClientFactory);

            return default;
        }

        #endregion
    }
}