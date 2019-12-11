using System.Threading;
using System.Threading.Tasks;
using MailServices.Services.Interfaces;
using MailWeb.Cqrs.Commands.ClientSettings;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.ClientSettings
{
    public class UpdateActiveMailServiceCommandHandler : IRequestHandler<UpdateActiveMailServiceCommand, bool>
    {
        #region Properties

        private readonly IMailServiceFactory _mailServiceFactory;

        #endregion

        #region Constructor

        public UpdateActiveMailServiceCommandHandler(IMailServiceFactory mailServiceFactory)
        {
            _mailServiceFactory = mailServiceFactory;
        }

        #endregion

        #region Methods

        public virtual Task<bool> Handle(UpdateActiveMailServiceCommand command, CancellationToken cancellationToken)
        {
            _mailServiceFactory
                .SetActiveMailService(command.MailServiceUniqueName);

            return Task.FromResult(true);
        }

        #endregion
    }
}