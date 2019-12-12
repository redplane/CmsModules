using System;
using System.Collections.Generic;
using System.Linq;
using MailManager.Services.Implementations;
using MailManager.Services.Interfaces;
using MailWeb.Models;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;

namespace MailWeb.Services.Implementations
{
    public class MailServiceFactory : BaseMailClientFactory
    {
        #region Constructor

        public MailServiceFactory(IEnumerable<IMailClient> mailServices,
            IRequestProfile requestProfile,
            MailManagementDbContext dbContext) : base(mailServices)
        {
            _requestProfile = requestProfile;
            _dbContext = dbContext;
        }

        #endregion

        #region Properties

        private readonly MailManagementDbContext _dbContext;

        private readonly IRequestProfile _requestProfile;

        #endregion

        #region Methods

        public override IMailClient GetActiveMailService()
        {
            // Find the client settings.
            var tenantId = _requestProfile.TenantId;

            // Find the client setting.
            var clientSetting = _dbContext.ClientSettings
                .FirstOrDefault(x => x.Id == tenantId);

            if (clientSetting == null)
                return _mailServices.FirstOrDefault();

            var mailServiceType = Type.GetType(clientSetting.ActiveMailService.Type) ??
                                  throw new InvalidOperationException();

            var mailService = _mailServices
                .FirstOrDefault(x => x.GetType() == mailServiceType);

            return mailService;
        }

        public override void SetActiveMailService(string uniqueName)
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