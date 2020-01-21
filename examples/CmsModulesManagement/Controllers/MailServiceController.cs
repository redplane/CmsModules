using System.Linq;
using System.Threading.Tasks;
using MailModule.Services.Interfaces;
using MailWeb.Cqrs.Commands;
using MailWeb.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/mail-client")]
    public class MailServiceController : Controller
    {
        #region Constructor

        public MailServiceController(IMailClientsManager mailClientsManager, IMediator mediator)
        {
            _mailClientsManager = mailClientsManager;
            _mediator = mediator;
        }

        #endregion

        #region Properties

        private readonly IMailClientsManager _mailClientsManager;

        private readonly IMediator _mediator;

        #endregion

        #region Methods

        [HttpGet("")]
        public virtual async Task<MailClientViewModel[]> GetMailClientsAsync()
        {
            var mailServices = await _mailClientsManager
                .GetMailClientsAsync();

            return mailServices
                .Select(mailService => new MailClientViewModel(mailService.UniqueName, mailService.DisplayName))
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