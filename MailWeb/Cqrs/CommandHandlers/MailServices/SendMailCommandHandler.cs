using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailManager.Models.Interfaces;
using MailManager.Services.Interfaces;
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
            var mailService = command.UseMailServiceName
                ? _mailServiceFactory
                    .GetMailService(command.MailServiceName)
                : _mailServiceFactory.GetActiveMailClient();

            if (mailService == null)
                return false;

            var mailSender = (IMailAddress) command.Sender;
            var recipients = command.Recipients?.Select(recipient => (IMailAddress) recipient).ToArray();
            var carbonCopies = command.CarbonCopies?.Select(carbonCopy => (IMailAddress) carbonCopy).ToArray();
            var blindCarbonCopies = command.BlindCarbonCopies?.Select(blindCarbonCopy => (IMailAddress) blindCarbonCopy)
                .ToArray();

            await mailService
                .SendMailAsync(mailSender, recipients, carbonCopies, blindCarbonCopies, command.Subject,
                    command.Content, command.IsHtml, command.AdditionalSubjectData, command.AdditionalContentData,
                    cancellationToken);

            return true;
        }

        #endregion
    }
}