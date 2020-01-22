using System;
using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.MailSettings;
using CmsModulesManagement.Models;
using CmsModulesManagement.Models.Entities;
using CmsModulesManagement.Models.Interfaces;
using CmsModulesShared.Models.MailHosts;
using MediatR;

namespace CmsModulesManagement.Cqrs.CommandHandlers.MailSettings
{
    public class AddMailSettingCommandHandler : IRequestHandler<AddMailSettingCommand, MailClientSetting>
    {
        #region Constructor

        public AddMailSettingCommandHandler(SiteDbContext dbContext, IRequestProfile profile)
        {
            _dbContext = dbContext;
            _profile = profile;
        }

        #endregion

        #region Methods

        public virtual async Task<MailClientSetting> Handle(AddMailSettingCommand command,
            CancellationToken cancellationToken)
        {
            var mailSetting = new MailClientSetting(Guid.NewGuid(), command.UniqueName);
            mailSetting.ClientId = _profile.TenantId;
            mailSetting.DisplayName = command.DisplayName;
            mailSetting.Timeout = command.Timeout;

            var mailHost = command.MailHost;
            if (mailHost is SmtpHost smtpHost)
                mailSetting.MailHost = smtpHost;
            else if (mailHost is MailGunHost mailGunHost)
                mailSetting.MailHost = mailGunHost;

            _dbContext.MailClientSettings
                .Add(mailSetting);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return mailSetting;
        }

        #endregion

        #region Properties

        private readonly SiteDbContext _dbContext;

        private readonly IRequestProfile _profile;

        #endregion
    }
}