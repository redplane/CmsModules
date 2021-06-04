using System.Threading;
using System.Threading.Tasks;
using CmsModules.ManagementUi.Cqrs.Queries;
using CmsModules.ManagementUi.Models;
using CmsModules.ManagementUi.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CmsModules.ManagementUi.Cqrs.QueryHandlers.SiteSettings
{
    public class GetClientSettingsQueryHandler : IRequestHandler<GetClientSettingsQuery, SiteSetting[]>
    {
        #region Properties

        private readonly SiteDbContext _dbContext;

        #endregion

        #region Constructor

        public GetClientSettingsQueryHandler(SiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual Task<SiteSetting[]> Handle(GetClientSettingsQuery query, CancellationToken cancellationToken)
        {
            return _dbContext
                .ClientSettings
                .ToArrayAsync(cancellationToken);
        }

        #endregion
    }
}