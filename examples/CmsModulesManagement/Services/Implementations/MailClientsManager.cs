using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CmsModulesShared.Models.MailHosts;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;
using MailWeb.Models;
using MailWeb.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Services.Implementations
{
    public class MailClientsManager : IMailClientsManager
    {
        #region Constructor

        public MailClientsManager(
            IRequestProfile requestProfile, IHttpClientFactory httpClientFactory,
            SiteDbContext dbContext)
        {
            _requestProfile = requestProfile;
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        #region Properties

        private readonly SiteDbContext _dbContext;

        private readonly IRequestProfile _requestProfile;

        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Methods

        public virtual async Task<IMailClient[]> GetMailClientsAsync(CancellationToken cancellationToken = default)
        {
            var mailClients = await _dbContext.MailClientSettings
                .ToListAsync(cancellationToken);

            return mailClients.Select(ToMailClient).ToArray();
        }

        public virtual async Task<IMailClient> GetMailClientAsync(string uniqueName, CancellationToken cancellationToken = default)
        {
            var mailClients = _dbContext.MailClientSettings
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
            var tenantId = _requestProfile.TenantId;

            // Find the client setting.
            var clientSetting = await _dbContext.ClientSettings
                .Where(x => x.Id == tenantId)
                .FirstOrDefaultAsync(cancellationToken);

            if (clientSetting == null)
                return null;

            var mailClientSettingUniqueName = clientSetting.ActiveMailClient;

            var mailClientSetting = await _dbContext.MailClientSettings
                .Where(x => x.UniqueName == mailClientSettingUniqueName)
                .FirstOrDefaultAsync(cancellationToken);

            return ToMailClient(mailClientSetting);
        }

        public virtual async Task MarkMailClientAsActiveAsync(string uniqueName,
            CancellationToken cancellationToken = default)
        {
            // Find the client settings.
            var tenantId = _requestProfile.TenantId;

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

            return default;
        }

        #endregion
    }
}