using CorsModule.Models.Interfaces;
using MediatR;

namespace CmsModules.ManagementUi.Cqrs.Queries.CorsPolicies
{
    public class GetCorsPolicyQuery : IRequest<ICorsPolicy>
    {
        #region Properties

        public string Name { get; set; }

        #endregion
    }
}
