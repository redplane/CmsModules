using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Queries;
using MailWeb.Models;
using MailWeb.ViewModels.MailSettings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Cqrs.QueryHandlers
{
    public class GetMailSettingsQueryHandler : IRequestHandler<GetMailSettingsQuery, MailSettingViewModel[]>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;
        
        #endregion
        
        #region Constructor
        
        public GetMailSettingsQueryHandler(MailManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #endregion
        
        #region Methods

        public Task<MailSettingViewModel[]> Handle(GetMailSettingsQuery request, CancellationToken cancellationToken)
        {
            return _dbContext
                .MailClientSettings
                .Select(x => new MailSettingViewModel(x))
                .ToArrayAsync(cancellationToken);
        }
        
        #endregion
    }
}