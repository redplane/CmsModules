using System;
using System.Threading.Tasks;
using MediatR;

namespace MailWeb.Cqrs.Commands.CorsPolicies
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
