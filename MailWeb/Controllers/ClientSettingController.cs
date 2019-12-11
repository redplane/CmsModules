using System.Threading.Tasks;
using MailWeb.Cqrs.Commands;
using MailWeb.Cqrs.Commands.ClientSettings;
using MailWeb.Cqrs.Queries;
using MailWeb.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/client-setting")]
    public class ClientSettingController : Controller
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public ClientSettingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        [HttpGet]
        public virtual Task<ClientSetting[]> GetClientSettingsAsync()
        {
            return _mediator.Send(new GetClientSettingsQuery());
        }

        [HttpPost]
        public virtual async Task<ClientSetting> AddClientSettingAsync(
            [FromBody] AddClientSettingCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("mail-service")]
        public virtual async Task<ActionResult> UpdateMailServiceAsync(
            [FromBody] UpdateActiveMailServiceCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        #endregion
    }
}