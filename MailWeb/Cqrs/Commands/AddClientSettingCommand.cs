using MailWeb.Models.Entities;
using MailWeb.Models.ValueObjects;
using MediatR;

namespace MailWeb.Cqrs.Commands
{
    public class AddClientSettingCommand : IRequest<ClientSetting>
    {
        #region Properties

        public string Name { get; set; }

        public string UniqueName { get; set; }

        #endregion
    }
}