using MediatR;

namespace MailWeb.Cqrs.Commands
{
    public class UpdateActiveMailServiceCommand : IRequest<bool>
    {
        #region Properties

        public string MailServiceUniqueName { get; set; }

        #endregion

        #region Command

        public UpdateActiveMailServiceCommand()
        {
        }


        #endregion
    }
}