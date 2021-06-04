using System;
using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Commands.ClientSettings;
using CmsModulesManagement.Models;
using CmsModulesManagement.Models.Entities;
using CmsModulesManagement.Services.Interfaces;
using MailModule.Services.Interfaces;
using MediatR;

namespace CmsModulesManagement.Cqrs.CommandHandlers.ClientSettings
{
    public class AddSiteSettingCommandHandler : IRequestHandler<AddClientSettingCommand, SiteSetting>
    {
        #region Constructor

        public AddSiteSettingCommandHandler(SiteDbContext dbContext, ISiteMailClientsService siteMailClientsService)
        {
            _dbContext = dbContext;
            _siteMailClientsService = siteMailClientsService;
        }

        #endregion

        #region Methods

        public virtual async Task<SiteSetting> Handle(AddClientSettingCommand command,
            CancellationToken cancellationToken)
        {
            //var activeMailService =
            //    await _mailClientsManager.GetMailClientAsync(command.UniqueName, cancellationToken);
            //if (activeMailService == null)
            //    throw new Exception($"Mails service named {command.UniqueName} is not found");

            var siteSetting = new SiteSetting(Guid.NewGuid());
            siteSetting.Name = command.Name;
            //siteSetting.ActiveMailClient = activeMailService.UniqueName;

            _dbContext.Add(siteSetting);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return siteSetting;
        }

        #endregion

        #region Properties

        private readonly SiteDbContext _dbContext;

        private readonly ISiteMailClientsService _siteMailClientsService;

        #endregion
    }
}