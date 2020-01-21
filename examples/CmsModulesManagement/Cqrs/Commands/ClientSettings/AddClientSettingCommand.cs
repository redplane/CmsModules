using MailWeb.Models.Entities;
using MediatR;

namespace MailWeb.Cqrs.Commands.ClientSettings
{
    public class AddClientSettingCommand : IRequest<SiteSetting>
    {
        #region Properties

        public string Name { get; set; }

        public string UniqueName { get; set; }

        #endregion
    }
}