using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.ClientSettings;
using CmsModulesManagement.Services.Interfaces;
using MailModule.Services.Interfaces;
using MediatR;

namespace CmsModulesManagement.Cqrs.CommandHandlers.ClientSettings
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