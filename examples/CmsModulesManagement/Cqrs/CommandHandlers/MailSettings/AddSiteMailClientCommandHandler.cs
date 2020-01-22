using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailModule.Models.Interfaces;
using MailWeb.Cqrs.Commands.MailSettings;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.Services.Interfaces;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.MailSettings
{
    public class AddSiteMailClientCommandHandler : IRequestHandler<AddSiteMailClientCommand, SiteMailClientSetting>
    {
        #region Constructor

        public AddSiteMailClientCommandHandler(ISiteMailClientsService siteMailClientsService, ITenant tenant)
        {
            _siteMailClientsService = siteMailClientsService;
            _tenant = tenant;
        }

        #endregion

        #region Methods

        public virtual async Task<SiteMailClientSetting> Handle(AddSiteMailClientCommand command,
            CancellationToken cancellationToken)
        {
            var carbonCopies = command
                .CarbonCopies
                ?.Select(x => (IMailAddress)x)
                .ToArray();

            var blindCarbonCopies = command
                .BlindCarbonCopies
                ?.Select(x => (IMailAddress)x)
                .ToArray();

            var addedSiteMailClientSetting = await _siteMailClientsService
                .AddSiteMailClientSettingAsync(_tenant.Id, command.UniqueName, command.DisplayName, command.MailHost, command.Timeout, carbonCopies,
                blindCarbonCopies, cancellationToken);

            return (SiteMailClientSetting)addedSiteMailClientSetting;
        }

        #endregion

        #region Properties

        private readonly ISiteMailClientsService _siteMailClientsService;

        private readonly ITenant _tenant;

        #endregion
    }
}
