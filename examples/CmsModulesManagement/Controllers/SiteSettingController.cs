using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.ClientSettings;
using CmsModulesManagement.Cqrs.Queries;
using CmsModulesManagement.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CmsModulesManagement.Controllers
{
    [Route("api/client-setting")]
    public class SiteSettingController : Controller
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public SiteSettingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        [HttpGet]
        public virtual Task<SiteSetting[]> GetClientSettingsAsync()
        {
            return _mediator.Send(new GetClientSettingsQuery());
        }

        [HttpPost]
        public virtual async Task<SiteSetting> AddClientSettingAsync(
            [FromBody] AddClientSettingCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}