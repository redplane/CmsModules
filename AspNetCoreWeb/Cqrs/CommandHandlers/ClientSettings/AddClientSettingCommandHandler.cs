using System;
using System.Threading;
using System.Threading.Tasks;
using MailClientAbstraction.Services.Interfaces;
using MailWeb.Cqrs.Commands.ClientSettings;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.ClientSettings
{
    public class AddClientSettingCommandHandler : IRequestHandler<AddClientSettingCommand, ClientSetting>
    {
        #region Constructor

        public AddClientSettingCommandHandler(MailManagementDbContext dbContext, IMailClientFactory mailServiceFactory)
        {
            _dbContext = dbContext;
            _mailServiceFactory = mailServiceFactory;
        }

        #endregion

        #region Methods

        public virtual async Task<ClientSetting> Handle(AddClientSettingCommand command,
            CancellationToken cancellationToken)
        {
            var activeMailService =
                await _mailServiceFactory.GetMailServiceAsync(command.UniqueName, cancellationToken);
            if (activeMailService == null)
                throw new Exception($"Mails service named {command.UniqueName} is not found");

            var clientSetting = new ClientSetting(Guid.NewGuid());
            clientSetting.Name = command.Name;
            clientSetting.ActiveMailClient = activeMailService.UniqueName;

            _dbContext.Add(clientSetting);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return clientSetting;
        }

        #endregion

        #region Properties

        private readonly MailManagementDbContext _dbContext;

        private readonly IMailClientFactory _mailServiceFactory;

        #endregion
    }
}