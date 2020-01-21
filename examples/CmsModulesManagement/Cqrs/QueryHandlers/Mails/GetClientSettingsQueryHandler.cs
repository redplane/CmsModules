using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Queries;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Cqrs.QueryHandlers.Mails
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