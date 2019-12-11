using MediatR;

namespace MailWeb.Cqrs.Commands.ClientSettings
{
    public class UpdateActiveMailServiceCommand : IRequest<bool>
    {
        #region Command

        #endregion

        #region Properties

        public string MailServiceUniqueName { get; set; }

        #endregion
    }
}