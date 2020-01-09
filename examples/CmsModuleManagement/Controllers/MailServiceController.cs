using System.Linq;
using System.Threading.Tasks;
using MailModule.Services.Interfaces;
using MailWeb.Cqrs.Commands;
using MailWeb.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/mail-service")]
    public class MailServiceController : Controller
    {
        #region Constructor

        public MailServiceController(IMailClientsManager mailServiceFactory, IMediator mediator)
        {
            _mailServiceFactory = mailServiceFactory;
            _mediator = mediator;
        }

        #endregion

        #region Properties

        private readonly IMailClientsManager _mailServiceFactory;

        private readonly IMediator _mediator;

        #endregion

        #region Methods

        [HttpGet("")]
        public virtual async Task<MailServiceViewModel[]> GetMailServicesAsync()
        {
            var mailServices = await _mailServiceFactory
                .GetMailClientsAsync();

            return mailServices
                .Select(mailService => new MailServiceViewModel(mailService.UniqueName, mailService.DisplayName))
                .ToArray();
        }

        [HttpPost("mail")]
        public virtual async Task<ActionResult> SendMailAsync([FromBody] SendMailCommand command)
        {
            if (command == null)
                command = new SendMailCommand();

            var hasMailSent = await _mediator.Send(command);
            if (!hasMailSent)
                return NotFound();

            return Ok();
        }

        #endregion
    }
}