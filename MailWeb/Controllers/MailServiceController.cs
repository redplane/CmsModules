using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailServices.Models.Interfaces;
using MailServices.Services.Interfaces;
using MailWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/mail-service")]
    public class MailServiceController : Controller
    {
        #region Properties

        private readonly IMailManagerService _mailManagerService;

        #endregion

        #region Constructor

        public MailServiceController(IMailManagerService mailManagerService)
        {
            _mailManagerService = mailManagerService;
        }

        #endregion

        #region Methods

        [HttpGet("")]
        public virtual Task<MailServiceViewModel[]> GetMailServicesAsync()
        {
            var mailServices = _mailManagerService
                .GetMailServices()
                .Select(mailService => new MailServiceViewModel(mailService.UniqueName, mailService.DisplayName))
                .ToArray();

            return Task.FromResult(mailServices);
        }

        [HttpPost("mail")]
        public virtual async Task<ActionResult> SendMailAsync([FromBody] SendMailViewModel command)
        {
            // Get mail service.
            var mailService = _mailManagerService
                .GetMailService(command.ServiceUniqueName);

            if (mailService == null)
                return NotFound();

            var mailSender = (IMailAddress) command.Sender;
            var recipients = command.Recipients?.Select(recipient => (IMailAddress) recipient).ToArray();
            var carbonCopies = command.CarbonCopies?.Select(carbonCopy => (IMailAddress) carbonCopy).ToArray();
            var blindCarbonCopies = command.BlindCarbonCopies?.Select(blindCarbonCopy => (IMailAddress) blindCarbonCopy).ToArray();

            await mailService
                .SendMailAsync(mailSender, recipients, carbonCopies, blindCarbonCopies, command.Subject,
                    command.Content, command.IsHtml, command.AdditionalSubjectData, command.AdditionalContentData);

            return Ok();
        }

        #endregion
    }
}