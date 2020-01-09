using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Queries;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Cqrs.QueryHandlers
{
    public class GetClientSettingsQueryHandler : IRequestHandler<GetClientSettingsQuery, ClientSetting[]>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;

        #endregion

        #region Constructor

        public GetClientSettingsQueryHandler(MailManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual Task<ClientSetting[]> Handle(GetClientSettingsQuery query, CancellationToken cancellationToken)
        {
            return _dbContext
                .ClientSettings
                .ToArrayAsync(cancellationToken);
        }

        #endregion
    }
}