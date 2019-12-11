using System.Threading.Tasks;
using MailWeb.Cqrs.Queries;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
{
    [Route("api/basic-mail-setting")]
    public class BasicMailSettingController : Controller
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public BasicMailSettingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        public virtual Task<BasicMailSettingViewModel[]> GetMailSettingsAsync()
        {
            var loadMailSettingsQuery = new GetBasicMailSettingsQuery();
            return _mediator.Send(loadMailSettingsQuery);
        }

        #endregion
    }
}