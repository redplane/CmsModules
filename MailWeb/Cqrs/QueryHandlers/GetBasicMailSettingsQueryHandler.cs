using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Queries;
using MailWeb.Models;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Cqrs.QueryHandlers
{
    public class GetBasicMailSettingsQueryHandler : IRequestHandler<GetBasicMailSettingsQuery, BasicMailSettingViewModel[]>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;
        
        #endregion
        
        #region Constructor
        
        public GetBasicMailSettingsQueryHandler(MailManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #endregion
        
        #region Methods

        public Task<BasicMailSettingViewModel[]> Handle(GetBasicMailSettingsQuery request, CancellationToken cancellationToken)
        {
            return _dbContext
                .BasicMailSettings
                .Select(x => new BasicMailSettingViewModel(x))
                .ToArrayAsync(cancellationToken);
        }
        
        #endregion
    }
}