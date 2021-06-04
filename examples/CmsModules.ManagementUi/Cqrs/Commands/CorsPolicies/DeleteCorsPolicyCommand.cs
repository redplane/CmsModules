using MediatR;

namespace CmsModules.ManagementUi.Cqrs.Commands.CorsPolicies
{
    public class DeleteCorsPolicyCommand : IRequest<bool>
    {
        #region Properties

        /// <summary>
        /// Name of policy that needs to be deleted.
        /// </summary>
        public string UniqueName { get; set; }

        #endregion
    }
}
