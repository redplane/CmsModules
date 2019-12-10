using System;
using System.Threading;
using System.Threading.Tasks;
using MailServices.Services.Interfaces;
using MailWeb.Cqrs.Commands;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MailWeb.Models.ValueObjects;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers
{
    public class AddClientSettingCommandHandler : IRequestHandler<AddClientSettingCommand, ClientSetting>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;

        private readonly IMailManagerService _mailManagerService;

        #endregion

        #region Constructor

        public AddClientSettingCommandHandler(MailManagementDbContext dbContext, IMailManagerService mailManagerService)
        {
            _dbContext = dbContext;
            _mailManagerService = mailManagerService;
        }

        #endregion

        #region Methods
        public virtual async Task<ClientSetting> Handle(AddClientSettingCommand command, CancellationToken cancellationToken)
        {
            var activeMailService = _mailManagerService.GetMailService(command.UniqueName);
            if (activeMailService == null)
                throw new Exception($"Mails service named {command.UniqueName} is not found");

            var clientSetting = new ClientSetting(Guid.NewGuid());
            clientSetting.Name = command.Name;
            clientSetting.ActiveMailService = new MailServiceValueObject(activeMailService.UniqueName, activeMailService.GetType().FullName);

            _dbContext.Add(clientSetting);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return clientSetting;
        }

        #endregion
    }
}