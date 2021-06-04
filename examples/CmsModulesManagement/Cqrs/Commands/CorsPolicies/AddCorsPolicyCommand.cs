using CorsModule.Models.Interfaces;
using MediatR;

namespace CmsModulesManagement.Cqrs.Commands.CorsPolicies
{
    public class AddCorsPolicyCommand : IRequest<ICorsPolicy>
    {
        #region Properties

        public string Name { get; set; }

        public string[] AllowedHeaders { get; set; }

        public string[] AllowedOrigins { get; set; }

        public string[] AllowedExposedHeaders { get; set; }

        public string[] AllowedMethods { get; set; }

        public bool AllowCredential { get; set; }

        #endregion
    }
}
