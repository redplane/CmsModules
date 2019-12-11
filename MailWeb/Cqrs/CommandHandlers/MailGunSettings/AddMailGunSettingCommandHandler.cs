using System;
using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands.MailGunSettings;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.MailGunSettings
{
    public class AddMailGunSettingCommandHandler : IRequestHandler<AddMailGunSettingCommand, IBasicMailSetting>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;

        #endregion

        #region Constructor

        public AddMailGunSettingCommandHandler(MailManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public Task<IBasicMailSetting> Handle(AddMailGunSettingCommand command, CancellationToken cancellationToken)
        {
            var mailGunSetting = new BasicMailSetting(Guid.NewGuid(), command.UniqueName);
            mailGunSetting.DisplayName = command.DisplayName;
            mailGunSetting.Timeout = command.Timeout;

            var mailGunHost = new MailGunHost();
            mailGunHost.ApiKey = command.ApiKey;
            mailGunHost.Domain = command.Domain;
            mailGunSetting.MailHost = mailGunHost;

            _dbContext.BasicMailSettings
                .Add(mailGunSetting);

            _dbContext.SaveChanges();
            return Task.FromResult((IBasicMailSetting) mailGunSetting);
        }

        #endregion
    }
}