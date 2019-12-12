using System;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands.MailSettings;
using MailWeb.Cqrs.Queries;
using MailWeb.ViewModels.MailSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/mail-setting")]
    public class MailSettingController : Controller
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public MailSettingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        [HttpGet]
        public virtual Task<MailSettingViewModel[]> GetMailSettingsAsync()
        {
            var loadMailSettingsQuery = new GetMailSettingsQuery();
            return _mediator.Send(loadMailSettingsQuery);
        }

        [HttpGet("mail-host-assemblies")]
        public virtual Task<string[]> GetMailSettingAssembliesAsync()
        {
            var loadMailSettingAssembliesQuery = new GetMailHostAssembliesQuery();
            return _mediator.Send(loadMailSettingAssembliesQuery);
        }

        [HttpPost("")]
        public virtual async Task<ActionResult> AddMailSettingAsync([FromBody] AddMailSettingCommand command)
        {
            if (command == null)
                command = new AddMailSettingCommand();

            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> EditMailSettingAsync([FromRoute] Guid id,
            [FromBody] EditMailSettingCommand command)
        {
            if (command == null)
                command = new EditMailSettingCommand();
            
            command.Id = id;
            var mailSetting = await _mediator.Send(command);
            return Ok(mailSetting);
        }
        #endregion
    }
}