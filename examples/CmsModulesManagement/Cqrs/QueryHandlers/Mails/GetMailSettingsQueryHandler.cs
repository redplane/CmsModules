using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Queries;
using CmsModulesManagement.Models;
using CmsModulesManagement.ViewModels.MailSettings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CmsModulesManagement.Cqrs.QueryHandlers.Mails
{
    public class GetMailSettingsQueryHandler : IRequestHandler<GetMailSettingsQuery, MailSettingViewModel[]>
    {
        #region Properties

        private readonly SiteDbContext _dbContext;

        #endregion

        #region Constructor

        public GetMailSettingsQueryHandler(SiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public Task<MailSettingViewModel[]> Handle(GetMailSettingsQuery request, CancellationToken cancellationToken)
        {
            return _dbContext
                .SiteMailClientSettings
                .Select(x => new MailSettingViewModel(x))
                .ToArrayAsync(cancellationToken);
        }

        #endregion
    }
}