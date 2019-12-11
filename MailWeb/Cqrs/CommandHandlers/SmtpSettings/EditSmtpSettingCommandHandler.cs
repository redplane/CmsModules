using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands.SmtpSettings;
using MailWeb.Models;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Cqrs.CommandHandlers.SmtpSettings
{
    public class EditSmtpSettingCommandHandler : IRequestHandler<EditSmtpSettingCommand, BasicMailSettingViewModel>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;

        #endregion

        #region Constructor

        public EditSmtpSettingCommandHandler(MailManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual async Task<BasicMailSettingViewModel> Handle(EditSmtpSettingCommand command,
            CancellationToken cancellationToken)
        {
            var hasModified = false;

            // Find the basic mail setting.
            var basicMailSetting = await _dbContext.BasicMailSettings
                .Where(x => x.Id == command.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (basicMailSetting == null)
                throw new Exception("Basic mail setting is not found");

            if (command.DisplayName != null && command.DisplayName.HasModified)
            {
                basicMailSetting.DisplayName = command.DisplayName.Value;
                hasModified = true;
            }

            if (command.Timeout != null && command.Timeout.HasModified)
            {
                basicMailSetting.Timeout = command.Timeout.Value;
                hasModified = true;
            }

            if (!hasModified)
                return new BasicMailSettingViewModel(basicMailSetting);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return new BasicMailSettingViewModel(basicMailSetting);
        }

        #endregion
    }
}