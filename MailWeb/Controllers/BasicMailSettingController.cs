using System;
using System.Linq;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands;
using MailWeb.Models;
using MailWeb.ViewModels;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Controllers
{
    [Route("api/basic-mail-setting")]
    public class BasicMailSettingController : Controller
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public BasicMailSettingController(MailManagementDbContext dbContext,
            IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

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
        public virtual async Task<ActionResult> AddBasicMailSettingAsync([FromBody] AddBasicMailSettingCommand command)
        {
            if (command == null)
                command = new AddBasicMailSettingCommand();

            var addedBasicMailSetting = await _mediator.Send(command);
            return Ok(addedBasicMailSetting);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> EditBasicMailSettingAsync([FromRoute] Guid id, 
            [FromBody] EditBasicMailSettingCommand command)
        {
            if (command == null)
                command = new EditBasicMailSettingCommand(id);

            var addedBasicMailSetting = await _mediator.Send(command);
            return Ok(addedBasicMailSetting);
        }

        #endregion
    }
}