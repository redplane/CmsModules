using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MailManager.Models.Interfaces;
using MailManager.Services.Implementations;
using MailManager.Services.Interfaces;
using MailWeb.Constants;
using MailWeb.Models;
using MailWeb.Models.Interfaces;
using MailWeb.Models.MailHosts;
using MailWeb.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Services.Implementations
{
    public class MailClientFactory : BaseMailClientFactory
    {
        #region Constructor

        public MailClientFactory(IEnumerable<IMailClient> mailServices,
            IRequestProfile requestProfile, IHttpClientFactory httpClientFactory,
            MailManagementDbContext dbContext) : base(mailServices)
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

        public override IMailClient GetActiveMailClient()
        {
            // Find the client settings.
            var tenantId = _requestProfile.TenantId;

            // Find the client setting.
            var clientSetting = _dbContext.ClientSettings
                .FirstOrDefault(x => x.Id == tenantId);

            if (clientSetting == null)
                return null;

            var mailClientSettingUniqueName = clientSetting.ActiveMailService.Name;
            var mailClientSetting = _dbContext.MailClientSettings
                .FirstOrDefault(x => x.UniqueName == mailClientSettingUniqueName);

            if (mailClientSetting == null)
                return null;

            if (mailClientSetting.MailHost == null)
                return null;

            if (mailClientSetting.MailHost is SmtpHost)
                return new OutlookMailClient(mailClientSetting);

            if (mailClientSetting.MailHost is MailGunHost)
                return new MailGunClient(mailClientSetting, _httpClientFactory);

            throw new NotImplementedException();
        }

        public override void SetActiveMailClient(string uniqueName)
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

            clientSetting.ActiveMailService = new MailServiceValueObject(uniqueName, mailService.GetType());
            _dbContext.SaveChanges();
        }

        #endregion
    }
}