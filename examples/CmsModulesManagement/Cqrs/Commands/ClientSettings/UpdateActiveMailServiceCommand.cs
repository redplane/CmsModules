using MediatR;

namespace MailWeb.Cqrs.Commands.ClientSettings
{
    public class UpdateActiveMailServiceCommand : IRequest<bool>
    {
        #region Properties

        public string MailServiceUniqueName { get; set; }

        #endregion

        #region Command

        #endregion
    }
}