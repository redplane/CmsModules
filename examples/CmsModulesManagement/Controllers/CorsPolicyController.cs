using System.Net;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;
using MailWeb.Cqrs.Commands.CorsPolicies;
using MailWeb.Cqrs.Queries.CorsPolicies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MailWeb.Controllers
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


        #endregion
    }
}
