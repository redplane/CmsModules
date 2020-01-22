using System;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.MailSettings;
using CmsModulesManagement.Cqrs.Queries;
using CmsModulesManagement.ViewModels.MailSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CmsModulesManagement.Controllers
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
        public virtual Task<MailSettingViewModel[]> GetMailClientSettingsAsync()
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
        public virtual async Task<ActionResult> AddMailClientSettingAsync([FromBody] AddMailSettingCommand command)
        {
            if (command == null)
                command = new AddMailSettingCommand();

            var addedMailSetting = await _mediator.Send(command);
            return Ok(addedMailSetting);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> EditMailClientSettingAsync([FromRoute] Guid id,
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