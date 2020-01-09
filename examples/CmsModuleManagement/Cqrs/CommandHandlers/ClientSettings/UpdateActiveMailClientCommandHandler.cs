using System.Threading;
using System.Threading.Tasks;
using MailModule.Services.Interfaces;
using MailWeb.Cqrs.Commands.ClientSettings;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.ClientSettings
{
    public class UpdateActiveMailClientCommandHandler : IRequestHandler<UpdateActiveMailServiceCommand, bool>
    {
        #region Properties

        private readonly IMailClientsManager _mailServiceFactory;

        #endregion

        #region Constructor

        public UpdateActiveMailClientCommandHandler(IMailClientsManager mailServiceFactory)
        {
            _mailServiceFactory = mailServiceFactory;
        }

        #endregion

        #region Methods

        public virtual async Task<bool> Handle(UpdateActiveMailServiceCommand command,
            CancellationToken cancellationToken)
        {
            await _mailServiceFactory
                .SetActiveMailClientAsync(command.MailServiceUniqueName, cancellationToken);

            return true;
        }

        #endregion
    }
}