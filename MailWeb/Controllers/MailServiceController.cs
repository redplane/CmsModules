using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailServices.Models.Interfaces;
using MailServices.Services.Interfaces;
using MailWeb.Cqrs.Commands;
using MailWeb.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/mail-service")]
    public class MailServiceController : Controller
    {
        #region Properties

        private readonly IMailManagerService _mailManagerService;

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public MailServiceController(IMailManagerService mailManagerService, IMediator mediator)
        {
            _mailManagerService = mailManagerService;
            _mediator = mediator;
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