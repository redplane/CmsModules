using System;
using System.Linq;
using CmsModules.ManagementUi.Constants;
using Microsoft.AspNetCore.Http;

namespace CmsModules.ManagementUi.Extensions
{
    public static class HttpContextExtensions
    {
        #region Methods

        public static Guid GetTenantId(this HttpContext httpContext)
        {
            var initialSiteId = httpContext.Request
                .Headers.Where(header => HttpHeaderConstants.SiteId.Equals(header.Key, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value.ToString())
                .FirstOrDefault();

            if (!Guid.TryParse(initialSiteId, out var siteId))
                return Guid.Empty;

            return siteId;
        }

        #endregion
    }
}