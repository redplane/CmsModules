using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmsModulesShared.Models.MailHosts;
using MailModule.Models.Interfaces;
using MailWeb.Cqrs.Commands.MailSettings;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.Services.Interfaces;
using MailWeb.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Cqrs.CommandHandlers.MailSettings
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