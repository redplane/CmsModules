using System;
using System.Net;
using System.Threading.Tasks;
using CmsModules.ManagementUi.Cqrs.Commands.CorsPolicies;
using CmsModules.ManagementUi.Cqrs.Queries.CorsPolicies;
using CorsModule.Models.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CmsModules.ManagementUi.Controllers
{
    [Route("api/cors-policy")]
    public class CorsPolicyController : Controller
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public CorsPolicyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        [HttpPost("")]
        public virtual async Task<ICorsPolicy> AddCorsPolicyAsync([FromBody] AddCorsPolicyCommand command)
        {
            if (command == null)
                command = new AddCorsPolicyCommand();

            var corsPolicy = await _mediator.Send(command);
            return corsPolicy;
        }

        [HttpDelete("")]
        public virtual async Task<HttpStatusCode> DeleteCorsPolicyAsync([FromQuery] DeleteCorsPolicyCommand command)
        {
            if (command == null)
                command = new DeleteCorsPolicyCommand();

            await _mediator.Send(command);
            return HttpStatusCode.OK;
        }

        [HttpGet("")]
        public virtual async Task<ICorsPolicy> GetCorsPolicyAsync([FromQuery] GetCorsPolicyQuery query)
        {
            if (query == null)
                query = new GetCorsPolicyQuery();

            return await _mediator.Send(query);
        }

        [HttpPut("{id}")]
        public virtual async Task<ICorsPolicy> UpdateCorsPolicyAsync([FromRoute] Guid id, [FromBody] UpdateCorsPolicyCommand command)
        {
            if (command == null)
                command = new UpdateCorsPolicyCommand();

            command.Id = id;
            return await _mediator.Send(command);
        }

        #endregion
    }
}
