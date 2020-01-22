using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;
using MailWeb.Cqrs.Commands;
using MailWeb.Services.Interfaces;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.MailServices
{
    public class SendMailCommandHandler : IRequestHandler<SendMailCommand, bool>
    {
        #region Properties

        private readonly ISiteMailClientsService _mailClientsManager;

        #endregion

        #region Constructor

        public SendMailCommandHandler(ISiteMailClientsService mailClientsManager)
        {
            _mailClientsManager = mailClientsManager;
        }

        #endregion

        #region Methods

        public virtual async Task<bool> Handle(SendMailCommand command, CancellationToken cancellationToken)
        {
            // Get mail service.
            IMailClient mailClient = null;

            if (command.UseMailServiceName)
                mailClient = await _mailClientsManager.GetMailClientAsync(command.MailServiceName, cancellationToken);
            else
                mailClient = await _mailClientsManager.GetActiveMailClientAsync(cancellationToken);

            if (mailClient == null)
                return false;

            var mailSender = (IMailAddress) command.Sender;
            var recipients = command.Recipients?.Select(recipient => (IMailAddress) recipient).ToArray();
            var carbonCopies = command.CarbonCopies?.Select(carbonCopy => (IMailAddress) carbonCopy).ToArray();
            var blindCarbonCopies = command.BlindCarbonCopies?.Select(blindCarbonCopy => (IMailAddress) blindCarbonCopy)
                .ToArray();

            await mailClient
                .SendMailAsync(mailSender, recipients, carbonCopies, blindCarbonCopies, command.Subject,
                    command.Content, command.IsHtml, command.AdditionalSubjectData, command.AdditionalContentData,
                    null,
                    cancellationToken);

            return true;
        }

        #endregion
    }
}