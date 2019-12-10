using System;
using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MailWeb.ViewModels;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers
{
    public class AddBasicMailSettingCommandHandler : IRequestHandler<AddBasicMailSettingCommand, BasicMailSettingViewModel>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;

        #endregion

        #region Constructor

        public AddBasicMailSettingCommandHandler(MailManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual async Task<BasicMailSettingViewModel> Handle(AddBasicMailSettingCommand command, CancellationToken cancellationToken)
        {
            var basicMailSetting = new BasicMailSetting(command.UniqueName);
            basicMailSetting.DisplayName = command.DisplayName;
            basicMailSetting.Timeout = command.Timeout;
            basicMailSetting.HostName = command.HostName;
            basicMailSetting.Port = command.Port;
            basicMailSetting.Ssl = command.Ssl;
            basicMailSetting.Credential = command.Credential;

            _dbContext.BasicMailSettings
                .Add(basicMailSetting);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return new BasicMailSettingViewModel(basicMailSetting);
        }

        #endregion
    }
}