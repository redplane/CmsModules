using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Models;
using CmsModulesManagement.Models.Entities;
using CmsModulesManagement.Models.Interfaces;
using CmsModulesManagement.Services.Implementations.MailClients;
using CmsModulesManagement.Services.Interfaces;
using CmsModulesManagement.ViewModels;
using CmsModulesShared.Models.MailHosts;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CmsModulesManagement.Services.Implementations
{
    public class SiteMailClientsService : ISiteMailClientsService
    {
        #region Constructor

        public SiteMailClientsService(
            ITenant tenant, IHttpClientFactory httpClientFactory,
            SiteDbContext dbContext)
        {
            _tenant = tenant;
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        #region Properties

        private readonly SiteDbContext _dbContext;

        private readonly ITenant _tenant;

        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Methods

        public virtual async Task<IMailClient[]> GetMailClientsAsync(CancellationToken cancellationToken = default)
        {
            var mailClients = await _dbContext.SiteMailClientSettings
                .ToListAsync(cancellationToken);

            return mailClients.Select(ToMailClient).ToArray();
        }

        public virtual async Task<IMailClient> GetMailClientAsync(string uniqueName, CancellationToken cancellationToken = default)
        {
            var mailClients = _dbContext.SiteMailClientSettings
                .Where(x => x.UniqueName == uniqueName);

            var mailClientSetting = await mailClients
                .FirstOrDefaultAsync(cancellationToken);

            if (mailClientSetting == null)
                return default;

            return ToMailClient(mailClientSetting);
        }

        public virtual async Task<IMailClient> GetActiveMailClientAsync(CancellationToken cancellationToken = default)
        {
            // Find the client settings.
            var tenantId = _tenant.Id;

            // Find the client setting.
            var clientSetting = await _dbContext.ClientSettings
                .Where(x => x.Id == tenantId)
                .FirstOrDefaultAsync(cancellationToken);

            if (clientSetting == null)
                return null;

            var mailClientSettingUniqueName = clientSetting.ActiveMailClient;

            var mailClientSetting = await _dbContext.SiteMailClientSettings
                .Where(x => x.UniqueName == mailClientSettingUniqueName)
                .FirstOrDefaultAsync(cancellationToken);

            return ToMailClient(mailClientSetting);
        }

        public virtual async Task MarkMailClientAsActiveAsync(string uniqueName,
            CancellationToken cancellationToken = default)
        {
            // Find the client settings.
            var tenantId = _tenant.Id;

            // Find the client setting.
            var clientSetting = _dbContext.ClientSettings
                .FirstOrDefault(x => x.Id == tenantId);

            if (clientSetting == null)
                return;

            // Find the mail service.
            var mailService = await GetMailClientAsync(uniqueName, cancellationToken);
            if (mailService == null)
                return;

            clientSetting.ActiveMailClient = uniqueName;
            _dbContext.SaveChanges();
        }

        public virtual async Task<IMailClientSetting> AddSiteMailClientSettingAsync(Guid tenantId, string uniqueName, string displayName, IMailHost mailHost, int timeout, IMailAddress[] carbonCopies, IMailAddress[] blindCarbonCopies,
            CancellationToken cancellationToken = default)
        {
            var siteMailClientSetting = new SiteMailClientSetting(Guid.NewGuid(), tenantId, uniqueName);
            siteMailClientSetting.DisplayName = displayName;
            if (mailHost is SmtpHost smtpHost)
                siteMailClientSetting.MailHost = smtpHost;
            else if (mailHost is MailGunHost mailGunHost)
                siteMailClientSetting.MailHost = mailGunHost;
            else
                throw new NotImplementedException($"{typeof(IMailHost).FullName} is not supported.");

            siteMailClientSetting.Timeout = timeout;
            siteMailClientSetting.CarbonCopies = carbonCopies;
            siteMailClientSetting.BlindCarbonCopies = blindCarbonCopies;

            _dbContext.SiteMailClientSettings
                .Add(siteMailClientSetting);

            await _dbContext
                .SaveChangesAsync(cancellationToken);

            return siteMailClientSetting;
        }
        public virtual async Task<IMailClientSetting> EditSiteMailClientSettingAsync(Guid id, Guid? tenantId,
            EditableFieldViewModel<string> displayName, IMailHost mailHost, EditableFieldViewModel<int> timeout,
            EditableFieldViewModel<IMailAddress[]> carbonCopies, EditableFieldViewModel<IMailAddress[]> blindCarbonCopies, 
            CancellationToken cancellationToken = default)
        {
            var siteMailClientSettings = _dbContext.SiteMailClientSettings.AsQueryable();
            siteMailClientSettings = siteMailClientSettings.Where(x => x.Id == id);

            if (tenantId != null)
                siteMailClientSettings = siteMailClientSettings.Where(x => x.TenantId == tenantId);

            // Find the site mail client setting.
            var siteMailClientSetting = await siteMailClientSettings
                .FirstOrDefaultAsync(cancellationToken);

            if (siteMailClientSetting == null)
                throw new Exception("Site mail client setting is not found.");

            var hasFieldChanged = false;
            if (displayName != null && displayName.HasModified)
            {
                siteMailClientSetting.DisplayName = displayName.Value;
                hasFieldChanged = true;
            }

            if (mailHost != null)
            {
                siteMailClientSetting.MailHost = mailHost;
                hasFieldChanged = true;
            }

            if (timeout != null && timeout.HasModified)
            {
                siteMailClientSetting.Timeout = timeout.Value;
                hasFieldChanged = true;
            }

            if (carbonCopies != null && carbonCopies.HasModified)
            {
                siteMailClientSetting.CarbonCopies = carbonCopies.Value;
                hasFieldChanged = true;
            }

            if (blindCarbonCopies != null && blindCarbonCopies.HasModified)
            {
                siteMailClientSetting.BlindCarbonCopies = blindCarbonCopies.Value;
                hasFieldChanged = true;
            }

            if (!hasFieldChanged)
                return siteMailClientSetting;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return siteMailClientSetting;
        }

        protected virtual IMailClient ToMailClient(IMailClientSetting mailClientSetting)
        {
            if (mailClientSetting == null)
                return null;

            if (mailClientSetting.MailHost == null)
                return null;

            if (mailClientSetting.MailHost is SmtpHost)
                return new OutlookMailClient(mailClientSetting);

            if (mailClientSetting.MailHost is MailGunHost)
                return new MailGunClient(mailClientSetting, _httpClientFactory);

            return null;
        }

        #endregion
    }
}