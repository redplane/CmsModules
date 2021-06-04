using CmsModules.ManagementUi.Models.Entities;
using MediatR;

namespace CmsModules.ManagementUi.Cqrs.Commands.ClientSettings
{
    public class AddClientSettingCommand : IRequest<SiteSetting>
    {
        #region Properties

        public string Name { get; set; }

        public string UniqueName { get; set; }

        #endregion
    }
}