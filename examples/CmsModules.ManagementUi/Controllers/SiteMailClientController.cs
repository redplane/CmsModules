using System;
using System.Threading.Tasks;
using CmsModules.ManagementUi.Cqrs.Commands;
using CmsModules.ManagementUi.Cqrs.Commands.MailSettings;
using CmsModules.ManagementUi.Cqrs.Queries.MailClientSettings;
using CmsModules.ManagementUi.ViewModels.MailSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CmsModules.ManagementUi.Controllers
{
    [Route("api/site-mail-client")]
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

        [HttpPost("mail-delivery")]
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