using CorsModule.Models.Interfaces;
using MediatR;

namespace CmsModulesManagement.Cqrs.Queries.CorsPolicies
{
    public class GetCorsPolicyQuery : IRequest<ICorsPolicy>
    {
        #region Properties

        public string Name { get; set; }

        #endregion
    }
}
