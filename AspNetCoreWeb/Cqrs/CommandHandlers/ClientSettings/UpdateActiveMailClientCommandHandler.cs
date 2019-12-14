using System.Threading;
using System.Threading.Tasks;
using MailManager.Services.Interfaces;
using MailWeb.Cqrs.Commands.ClientSettings;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.ClientSettings
{
    public class UpdateActiveMailClientCommandHandler : IRequestHandler<UpdateActiveMailServiceCommand, bool>
    {
        #region Properties

        private readonly IMailClientFactory _mailServiceFactory;

        #endregion

        #region Constructor

        public UpdateActiveMailClientCommandHandler(IMailClientFactory mailServiceFactory)
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