using System.Linq;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands;
using CmsModulesManagement.Services.Interfaces;
using CmsModulesManagement.ViewModels;
using MailModule.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CmsModulesManagement.Controllers
{
    [Route("api/mail-client")]
    public class MailServiceController : Controller
    {
        #region Constructor

        public MailServiceController(ISiteMailClientsService mailClientsManager, IMediator mediator)
        {
            _mailClientsManager = mailClientsManager;
            _mediator = mediator;
        }

        #endregion

        #region Properties

        private readonly ISiteMailClientsService _mailClientsManager;

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