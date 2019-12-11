using System.Threading.Tasks;
using MailWeb.Cqrs.Commands.MailGunSettings;
using MailWeb.Models.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/mail-gun")]
    public class MailGunSettingController : Controller
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public MailGunSettingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        [HttpPost("")]
        public virtual async Task<IBasicMailSetting> AddMailGunSettingAsync([FromBody] AddMailGunSettingCommand command)
        {
            if (command == null)
                command = new AddMailGunSettingCommand();

            return await _mediator.Send(command);
        }

        #endregion
    }
}