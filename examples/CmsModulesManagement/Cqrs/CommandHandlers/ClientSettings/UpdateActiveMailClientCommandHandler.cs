using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.ClientSettings;
using MailModule.Services.Interfaces;
using MediatR;

namespace CmsModulesManagement.Cqrs.CommandHandlers.ClientSettings
{
    public class UpdateActiveMailClientCommandHandler : IRequestHandler<UpdateActiveMailServiceCommand, bool>
    {
        #region Properties

        private readonly IMailClientsManager _mailClientsManager;

        #endregion

        #region Constructor

        public UpdateActiveMailClientCommandHandler(IMailClientsManager mailClientsManager)
        {
            _mailClientsManager = mailClientsManager;
        }

        #endregion

        #region Methods

        public virtual async Task<bool> Handle(UpdateActiveMailServiceCommand command,
            CancellationToken cancellationToken)
        {
            await _mailClientsManager
                .MarkMailClientAsActiveAsync(command.MailServiceUniqueName, cancellationToken);

            return true;
        }

        #endregion
    }
}