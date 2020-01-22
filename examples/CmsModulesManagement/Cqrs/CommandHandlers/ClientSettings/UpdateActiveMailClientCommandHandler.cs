using System.Threading;
using System.Threading.Tasks;
using MailModule.Services.Interfaces;
using MailWeb.Cqrs.Commands.ClientSettings;
using MailWeb.Services.Interfaces;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.ClientSettings
{
    public class UpdateActiveMailClientCommandHandler : IRequestHandler<UpdateActiveMailServiceCommand, bool>
    {
        #region Properties

        private readonly ISiteMailClientsService _siteMailClientsService;

        #endregion

        #region Constructor

        public UpdateActiveMailClientCommandHandler(ISiteMailClientsService siteMailClientsService)
        {
            _siteMailClientsService = siteMailClientsService;
        }

        #endregion

        #region Methods

        public virtual async Task<bool> Handle(UpdateActiveMailServiceCommand command,
            CancellationToken cancellationToken)
        {
            await _siteMailClientsService
                .MarkMailClientAsActiveAsync(command.MailServiceUniqueName, cancellationToken);

            return true;
        }

        #endregion
    }
}