using System;
using System.Linq;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands.SmtpSettings;
using MailWeb.Models;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Controllers
{
    [Route("api/smtp-setting")]
    public class SmtpSettingController : Controller
    {
        #region Constructor

        public SmtpSettingController(MailManagementDbContext dbContext,
            IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        #endregion

        #region Properties

        private readonly MailManagementDbContext _dbContext;

        private readonly IMediator _mediator;

        #endregion

        #region Methods

        [HttpGet("")]
        public virtual async Task<ActionResult> GetBasicMailSettingsAsync()
        {
            var basicMailSettings = await _dbContext.BasicMailSettings
                .Select(x => new BasicMailSettingViewModel(x))
                .ToListAsync();

            return Ok(basicMailSettings);
        }

        [HttpPost("")]
        public virtual async Task<ActionResult> AddBasicMailSettingAsync([FromBody] AddSmtpSettingCommand command)
        {
            if (command == null)
                command = new AddSmtpSettingCommand();

            var addedBasicMailSetting = await _mediator.Send(command);
            return Ok(addedBasicMailSetting);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> EditBasicMailSettingAsync([FromRoute] Guid id,
            [FromBody] EditSmtpSettingCommand command)
        {
            if (command == null)
                command = new EditSmtpSettingCommand();

            command.Id = id;

            var addedBasicMailSetting = await _mediator.Send(command);
            return Ok(addedBasicMailSetting);
        }

        #endregion
    }
}