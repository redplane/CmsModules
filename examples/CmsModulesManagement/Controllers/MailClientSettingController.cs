using System;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands.MailSettings;
using MailWeb.Cqrs.Queries;
using MailWeb.ViewModels.MailSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/mail-client-setting")]
    public class MailClientSettingController : Controller
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public MailClientSettingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        [HttpGet]
        public virtual Task<MailSettingViewModel[]> GetSiteMailClientSettingsAsync()
        {
            var loadMailSettingsQuery = new GetMailSettingsQuery();
            return _mediator.Send(loadMailSettingsQuery);
        }

        [HttpPost("")]
        public virtual async Task<ActionResult> AddSiteMailClientSettingAsync([FromBody] AddSiteMailClientCommand command)
        {
            if (command == null)
                command = new AddSiteMailClientCommand();

            var addedMailSetting = await _mediator.Send(command);
            return Ok(addedMailSetting);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> EditSiteMailClientSettingAsync([FromRoute] Guid id,
            [FromBody] EditSiteMailSettingCommand command)
        {
            if (command == null)
                command = new EditSiteMailSettingCommand();

            command.Id = id;
            var mailSetting = await _mediator.Send(command);
            return Ok(mailSetting);
        }

        #endregion
    }
}