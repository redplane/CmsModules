using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailClients.Models.Interfaces;
using MailClients.Services.Interfaces;
using MailWeb.Cqrs.Commands;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.MailServices
{
    public class SendMailCommandHandler : IRequestHandler<SendMailCommand, bool>
    {
        #region Properties

        private readonly IMailClientFactory _mailServiceFactory;

        #endregion

        #region Constructor

        public SendMailCommandHandler(IMailClientFactory mailServiceFactory)
        {
            _mailServiceFactory = mailServiceFactory;
        }

        #endregion

        #region Methods

        public virtual async Task<bool> Handle(SendMailCommand command, CancellationToken cancellationToken)
        {
            // Get mail service.
            IMailClient mailClient = null;

            if (command.UseMailServiceName)
                mailClient = await _mailServiceFactory.GetMailServiceAsync(command.MailServiceName, cancellationToken);
            else
                mailClient = await _mailServiceFactory.GetActiveMailClientAsync(cancellationToken);

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