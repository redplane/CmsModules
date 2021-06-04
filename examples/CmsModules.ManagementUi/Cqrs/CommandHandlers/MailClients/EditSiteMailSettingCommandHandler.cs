using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmsModules.ManagementUi.Cqrs.Commands.MailSettings;
using CmsModules.ManagementUi.Models.Entities;
using CmsModules.ManagementUi.Models.Interfaces;
using CmsModules.ManagementUi.Services.Interfaces;
using CmsModules.ManagementUi.ViewModels;
using MailModule.Models.Interfaces;
using MediatR;

namespace CmsModules.ManagementUi.Cqrs.CommandHandlers.MailClients
{
    public class EditSiteMailSettingCommandHandler : IRequestHandler<EditSiteMailSettingCommand, SiteMailClientSetting>
    {
        #region Properties

        private readonly ISiteMailClientsService _siteMailClientsService;

        private readonly ITenant _tenant;

        #endregion

        #region Constructor

        public EditSiteMailSettingCommandHandler(ISiteMailClientsService siteMailClientsService, ITenant tenant)
        {
            _siteMailClientsService = siteMailClientsService;
            _tenant = tenant;
        }

        #endregion

        #region Methods

        public virtual async Task<SiteMailClientSetting> Handle(EditSiteMailSettingCommand command,
            CancellationToken cancellationToken)
        {
            EditableFieldViewModel<IMailAddress[]> carbonCopies = null;
            EditableFieldViewModel<IMailAddress[]> blindCarbonCopies = null;

            if (command.CarbonCopies != null)
            {
                carbonCopies = new EditableFieldViewModel<IMailAddress[]>();
                carbonCopies.Value = command.CarbonCopies.Value?.Select(x => (IMailAddress)x).ToArray();
                carbonCopies.HasModified = command.CarbonCopies.HasModified;
            }

            if (command.BlindCarbonCopies != null)
            {
                blindCarbonCopies = new EditableFieldViewModel<IMailAddress[]>();
                blindCarbonCopies.Value = command.BlindCarbonCopies.Value?.Select(x => (IMailAddress)x).ToArray();
                blindCarbonCopies.HasModified = command.BlindCarbonCopies.HasModified;
            }

            var updatedSiteMailClientSetting = await _siteMailClientsService
                .EditSiteMailClientSettingAsync(command.Id, _tenant.Id, command.DisplayName, command.MailHost,
                command.Timeout, carbonCopies, blindCarbonCopies, cancellationToken);

            return (SiteMailClientSetting) updatedSiteMailClientSetting;
        }

        #endregion
    }
}