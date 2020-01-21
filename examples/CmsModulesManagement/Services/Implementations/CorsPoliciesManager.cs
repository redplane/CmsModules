using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;
using CorsModule.Services.Interfaces;
using MailWeb.Enums;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Services.Implementations
{
    public class CorsPoliciesManager : ICorsPoliciesManager
    {
        #region Properties

        private readonly SiteDbContext _dbContext;

        private readonly ITenant _tenant;

        #endregion

        #region Constructor

        public CorsPoliciesManager(SiteDbContext dbContext, ITenant tenant)
        {
            _dbContext = dbContext;
            _tenant = tenant;
        }

        #endregion

        #region Methods

        public virtual Task MarkCorsPolicyAsActiveAsync(ICorsPolicy corsPolicy, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task<ICorsPolicy> AddCorsPolicyAsync(ICorsPolicy corsPolicy, CancellationToken cancellationToken = default)
        {
            var initialCorsPolicy = new CorsPolicy(Guid.NewGuid(), corsPolicy);
            initialCorsPolicy.SiteId = _tenant.Id;

            _dbContext
                .CorsPolicies
                .Add(initialCorsPolicy);

            await _dbContext
                .SaveChangesAsync(cancellationToken);

            return initialCorsPolicy;
        }

        public virtual async Task DeleteCorsPolicyAsync(string uniqueName, CancellationToken cancellationToken = default)
        {
            var corsPolicies = _dbContext.CorsPolicies.AsQueryable();
            var targetedCorsPolicy = await corsPolicies.Where(x => x.Name.Equals(uniqueName))
                .FirstOrDefaultAsync(cancellationToken);

            if (targetedCorsPolicy == null)
                return;

            targetedCorsPolicy
                .Availability = MasterItemAvailabilities.Available;

            await _dbContext
                .SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<ICorsPolicy[]> GetInUseCorsPoliciesAsync(CancellationToken cancellationToken = default)
        {
            var corsPolicies = _dbContext.CorsPolicies
                .AsQueryable();

            var siteCorsPolicies = await corsPolicies
                .Where(x => (x.SiteId == _tenant.Id || x.SiteId == null) && x.Availability == MasterItemAvailabilities.Available)
                .OrderBy(x => x.SiteId)
                .ToArrayAsync(cancellationToken);

            return siteCorsPolicies
                .Select(x => (ICorsPolicy)x)
                .ToArray();
        }

        public virtual async Task<ICorsPolicy> GetCorsPolicyAsync(string uniqueName, CancellationToken cancellationToken = default)
        {
            var corsPolicies = _dbContext.CorsPolicies.AsQueryable();
            var matchedCorsPolicy = await corsPolicies.Where(x => x.Name.Equals(uniqueName))
                 .FirstOrDefaultAsync(cancellationToken);

            return matchedCorsPolicy;
        }

        public virtual async Task<ICorsPolicy[]> GetCorsPoliciesAsync(int skip, int? take,
            CancellationToken cancellationToken = default)
        {
            var corsPolicies = _dbContext.CorsPolicies
                .AsQueryable();

            corsPolicies = corsPolicies
                .Skip(skip);

            if (take != null && take > 0)
                corsPolicies = corsPolicies.Take(take.Value);

            var loadedCorsPolicies = await corsPolicies
                .ToListAsync(cancellationToken);

            return loadedCorsPolicies
                .Select(x => (ICorsPolicy)x)
            .ToArray();
        }


        #endregion
    }
}