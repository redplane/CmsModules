using CmsModulesManagement.Models.Entities;
using MediatR;

namespace CmsModulesManagement.Cqrs.Commands.ClientSettings
{
    public class AddClientSettingCommand : IRequest<SiteSetting>
    {
        #region Properties

        public string Name { get; set; }

        public string UniqueName { get; set; }

        #endregion
    }
}